using CRC.WebPortal.Application.Common.Dtos;
using CRC.WebPortal.Domain.Entities;
using CRC.WebPortal.Domain.Interfaces;
using CRC.Common.Models;
using MediatR;

namespace CRC.WebPortal.Application.Features.Units.Commands.CreateUnit;

public class CreateUnitCommandHandler : IRequestHandler<CreateUnitCommand, BaseResponse<UnitDto>>
{
    private readonly IRepository<Domain.Entities.Unit> _unitRepository;
    private readonly IRepository<Building> _buildingRepository;
    private readonly IRepository<UnitType> _unitTypeRepository;

    public CreateUnitCommandHandler(
        IRepository<Domain.Entities.Unit> unitRepository,
        IRepository<Building> buildingRepository,
        IRepository<UnitType> unitTypeRepository)
    {
        _unitRepository = unitRepository;
        _buildingRepository = buildingRepository;
        _unitTypeRepository = unitTypeRepository;
    }

    public async Task<BaseResponse<UnitDto>> Handle(CreateUnitCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Validate building exists
            var building = await _buildingRepository.GetFirstOrDefaultAsync(b => b.Id == request.Request.BuildingId && !b.IsDeleted);
            if (building == null)
            {
                return BaseResponse<UnitDto>.Failure("Building not found.");
            }

            // Validate unit type exists if provided
            UnitType? unitType = null;
            if (request.Request.UnitTypeId.HasValue)
            {
                unitType = await _unitTypeRepository.GetFirstOrDefaultAsync(ut => ut.Id == request.Request.UnitTypeId.Value && !ut.IsDeleted);
                if (unitType == null)
                {
                    return BaseResponse<UnitDto>.Failure("Unit type not found.");
                }
            }

            // Check if unit number already exists in the building
            var existingUnit = await _unitRepository.GetFirstOrDefaultAsync(
                u => u.BuildingId == request.Request.BuildingId && 
                     u.UnitNumber == request.Request.UnitNumber && 
                     !u.IsDeleted);

            if (existingUnit != null)
            {
                return BaseResponse<UnitDto>.Failure("Unit number already exists in this building.");
            }

            // Create unit entity
            var unit = new Domain.Entities.Unit
            {
                UnitNumber = request.Request.UnitNumber.Trim(),
                Floor = request.Request.Floor,
                Position = request.Request.Position,
                BuildingId = request.Request.BuildingId,
                UnitTypeId = request.Request.UnitTypeId
            };

            // Add unit to database
            await _unitRepository.AddAsync(unit);
            await _unitRepository.SaveChangesAsync();

            // Create DTO response
            var unitDto = new UnitDto
            {
                Id = unit.Id,
                UnitNumber = unit.UnitNumber,
                UnitType = unitType?.Name ?? "",
                FloorNumber = unit.Floor,
                BuildingName = building.Name,
                ProjectId = building.ProjectId,
                CreatedAt = unit.CreatedAt,
                Position = unit.Position,
                BuildingId = unit.BuildingId
            };

            return BaseResponse<UnitDto>.Success(unitDto, "Unit created successfully.");
        }
        catch (Exception ex)
        {
            return BaseResponse<UnitDto>.Failure($"Error creating unit: {ex.Message}");
        }
    }
}
