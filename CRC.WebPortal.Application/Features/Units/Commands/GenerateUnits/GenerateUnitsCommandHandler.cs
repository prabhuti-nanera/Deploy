using CRC.WebPortal.Application.Common.Dtos;
using CRC.WebPortal.Domain.Entities;
using CRC.WebPortal.Domain.Interfaces;
using CRC.Common.Models;
using MediatR;

namespace CRC.WebPortal.Application.Features.Units.Commands.GenerateUnits;

public class GenerateUnitsCommandHandler : IRequestHandler<GenerateUnitsCommand, BaseResponse<List<UnitDto>>>
{
    private readonly IRepository<Domain.Entities.Unit> _unitRepository;
    private readonly IRepository<Building> _buildingRepository;
    private readonly IRepository<UnitType> _unitTypeRepository;
    private readonly IRepository<Project> _projectRepository;

    public GenerateUnitsCommandHandler(
        IRepository<Domain.Entities.Unit> unitRepository,
        IRepository<Building> buildingRepository,
        IRepository<UnitType> unitTypeRepository,
        IRepository<Project> projectRepository)
    {
        _unitRepository = unitRepository;
        _buildingRepository = buildingRepository;
        _unitTypeRepository = unitTypeRepository;
        _projectRepository = projectRepository;
    }

    public async Task<BaseResponse<List<UnitDto>>> Handle(GenerateUnitsCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Validate project exists
            var project = await _projectRepository.GetFirstOrDefaultAsync(p => p.Id == request.Request.ProjectId && !p.IsDeleted);
            if (project == null)
            {
                return BaseResponse<List<UnitDto>>.Failure("Project not found.");
            }

            // Get all buildings for this project
            var buildings = await _buildingRepository.GetAllAsync(b => b.ProjectId == request.Request.ProjectId && !b.IsDeleted);
            var buildingDict = buildings.ToDictionary(b => b.Name, b => b);

            // Get all unit types for this project
            var unitTypes = await _unitTypeRepository.GetAllAsync(ut => ut.ProjectId == request.Request.ProjectId && !ut.IsDeleted);
            var unitTypeDict = unitTypes.ToDictionary(ut => ut.Name, ut => ut);

            // Clear existing units for this project
            var existingUnits = await _unitRepository.GetAllAsync(u => u.Building.ProjectId == request.Request.ProjectId && !u.IsDeleted);
            foreach (var existingUnit in existingUnits)
            {
                existingUnit.IsDeleted = true;
                existingUnit.UpdateTimestamps();
            }
            await _unitRepository.SaveChangesAsync();

            var createdUnits = new List<UnitDto>();
            var unitsToCreate = new List<Domain.Entities.Unit>();

            // Process each unit data
            foreach (var unitData in request.Request.UnitsData)
            {
                Console.WriteLine($"API Processing: Unit={unitData.UnitNumber}, Floor={unitData.FloorNumber}, Building={unitData.BuildingName}");
                
                if (!buildingDict.TryGetValue(unitData.BuildingName, out var building))
                {
                    continue; // Skip if building not found
                }

                // Find unit type
                UnitType? unitType = null;
                if (!string.IsNullOrEmpty(unitData.UnitType) && unitTypeDict.TryGetValue(unitData.UnitType, out var foundUnitType))
                {
                    unitType = foundUnitType;
                }

                // Create unit entity
                var unit = new Domain.Entities.Unit
                {
                    UnitNumber = unitData.UnitNumber.Trim(),
                    Floor = unitData.FloorNumber,
                    Position = unitData.Position,
                    BuildingId = building.Id,
                    UnitTypeId = unitType?.Id
                };

                unitsToCreate.Add(unit);

                // Create DTO for response
                var unitDto = new UnitDto
                {
                    UnitNumber = unit.UnitNumber,
                    UnitType = unitType?.Name ?? "",
                    FloorNumber = unit.Floor,
                    BuildingName = building.Name,
                    ProjectId = building.ProjectId,
                    Position = unit.Position,
                    BuildingId = unit.BuildingId,
                    CreatedAt = DateTime.UtcNow
                };

                createdUnits.Add(unitDto);
            }

            // Bulk insert units
            if (unitsToCreate.Any())
            {
                await _unitRepository.AddRangeAsync(unitsToCreate);
                await _unitRepository.SaveChangesAsync();

                // Update DTOs with generated IDs
                for (int i = 0; i < unitsToCreate.Count; i++)
                {
                    createdUnits[i].Id = unitsToCreate[i].Id;
                }
            }

            return BaseResponse<List<UnitDto>>.Success(createdUnits, $"Successfully generated {createdUnits.Count} units.");
        }
        catch (Exception ex)
        {
            return BaseResponse<List<UnitDto>>.Failure($"Error generating units: {ex.Message}");
        }
    }
}
