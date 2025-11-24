using CRC.WebPortal.Application.Common.Dtos;
using CRC.WebPortal.Domain.Entities;
using CRC.WebPortal.Domain.Interfaces;
using CRC.Common.Models;
using MediatR;
using CRC.WebPortal.Application.Common;
using CRC.WebPortal.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CRC.WebPortal.Application.Features.Projects.Commands.UpdateProject;

public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, BaseResponse<CRC.WebPortal.Shared.Dtos.ProjectDto>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMediator _mediator;

    public UpdateProjectCommandHandler(ApplicationDbContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }

    public async Task<BaseResponse<CRC.WebPortal.Shared.Dtos.ProjectDto>> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Find project in database
            var project = await _context.Projects
                .Where(p => p.Id == request.Request.Id && !p.IsDeleted)
                .FirstOrDefaultAsync(cancellationToken);
                
            if (project == null)
            {
                return BaseResponse<CRC.WebPortal.Shared.Dtos.ProjectDto>.Failure("Project not found.");
            }

            // Update project properties
            project.Name = request.Request.Name;
            project.Address = request.Request.Address;
            project.Type = request.Request.Type;
            project.AdminName = request.Request.AdminName;
            project.AdminMobile = request.Request.AdminMobile;
            project.UpdatedAt = DateTime.UtcNow;

            // Update buildings if provided (including empty array to delete all buildings)
            if (request.Request.Buildings != null)
            {
                var existingBuildings = await _context.Buildings
                    .Where(b => b.ProjectId == project.Id && !b.IsDeleted)
                    .ToListAsync(cancellationToken);

                // Get building names from the request
                var requestBuildingNames = request.Request.Buildings.Select(b => b.Name).ToHashSet();

                // Mark buildings not in the request as deleted
                foreach (var existingBuilding in existingBuildings)
                {
                    if (!requestBuildingNames.Contains(existingBuilding.Name))
                    {
                        existingBuilding.IsDeleted = true;
                        existingBuilding.UpdatedAt = DateTime.UtcNow;
                    }
                }

                // Update or add buildings from the request
                foreach (var buildingRequest in request.Request.Buildings)
                {
                    var existingBuilding = existingBuildings.FirstOrDefault(b => b.Name == buildingRequest.Name);
                    if (existingBuilding != null)
                    {
                        // Update existing building
                        existingBuilding.Type = buildingRequest.Type;
                        existingBuilding.Floors = buildingRequest.Floors;
                        existingBuilding.UnitsPerFloor = buildingRequest.UnitsPerFloor;
                        existingBuilding.UpdatedAt = DateTime.UtcNow;
                    }
                    else
                    {
                        // Add new building
                        var building = new Building
                        {
                            Name = buildingRequest.Name,
                            Type = buildingRequest.Type,
                            Floors = buildingRequest.Floors,
                            UnitsPerFloor = buildingRequest.UnitsPerFloor,
                            ProjectId = project.Id,
                            CreatedAt = DateTime.UtcNow
                        };
                        _context.Buildings.Add(building);
                    }
                }
            }

            // Update unit types if provided
            if (request.Request.UnitTypes?.Any() == true)
            {
                var existingUnitTypes = await _context.UnitTypes
                    .Where(ut => ut.ProjectId == project.Id && !ut.IsDeleted)
                    .ToListAsync(cancellationToken);

                // Update existing unit types and mark extras as deleted
                for (int i = 0; i < Math.Max(existingUnitTypes.Count, request.Request.UnitTypes.Count); i++)
                {
                    if (i < request.Request.UnitTypes.Count)
                    {
                        if (i < existingUnitTypes.Count)
                        {
                            // Update existing unit type
                            existingUnitTypes[i].Name = request.Request.UnitTypes[i];
                            existingUnitTypes[i].DisplayOrder = i;
                            existingUnitTypes[i].UpdatedAt = DateTime.UtcNow;
                        }
                        else
                        {
                            // Add new unit type
                            var unitType = new UnitType
                            {
                                Name = request.Request.UnitTypes[i],
                                Description = string.Empty,
                                DisplayOrder = i,
                                ProjectId = project.Id,
                                CreatedAt = DateTime.UtcNow
                            };
                            _context.UnitTypes.Add(unitType);
                        }
                    }
                    else if (i < existingUnitTypes.Count)
                    {
                        // Mark extra unit types as deleted (soft delete)
                        existingUnitTypes[i].IsDeleted = true;
                        existingUnitTypes[i].UpdatedAt = DateTime.UtcNow;
                    }
                }
            }

            // Update numbering pattern if provided
            if (request.Request.NumberingPattern != null)
            {
                // Remove existing numbering pattern and rows
                var existingPattern = await _context.NumberingPatterns
                    .Include(np => np.PatternRows)
                    .Where(np => np.ProjectId == project.Id)
                    .FirstOrDefaultAsync(cancellationToken);

                if (existingPattern != null)
                {
                    _context.NumberingPatternRows.RemoveRange(existingPattern.PatternRows);
                    _context.NumberingPatterns.Remove(existingPattern);
                }

                // Add new numbering pattern
                var numberingPattern = new NumberingPattern
                {
                    StartFloorNumber = request.Request.NumberingPattern.StartFloorNumber,
                    UnitNameDigits = request.Request.NumberingPattern.UnitNameDigits,
                    ApplyTo = request.Request.NumberingPattern.ApplyTo,
                    ProjectId = project.Id,
                    CreatedAt = DateTime.UtcNow
                };
                _context.NumberingPatterns.Add(numberingPattern);

                // Add pattern rows
                if (request.Request.NumberingPattern.PatternRows?.Any() == true)
                {
                    foreach (var rowRequest in request.Request.NumberingPattern.PatternRows)
                    {
                        var patternRow = new NumberingPatternRow
                        {
                            UnitType = rowRequest.UnitType,
                            FirstDigit = rowRequest.FirstDigit,
                            SecondDigit = rowRequest.SecondDigit,
                            ThirdDigit = rowRequest.ThirdDigit,
                            CustomFirstDigit = rowRequest.CustomFirstDigit,
                            CustomSecondDigit = rowRequest.CustomSecondDigit,
                            CustomThirdDigit = rowRequest.CustomThirdDigit,
                            Result = rowRequest.Result,
                            DigitsJson = !string.IsNullOrEmpty(rowRequest.DigitsJson) ? rowRequest.DigitsJson : "[]",
                            CustomDigitsJson = !string.IsNullOrEmpty(rowRequest.CustomDigitsJson) ? rowRequest.CustomDigitsJson : "[]",
                            OriginalDigitCount = rowRequest.OriginalDigitCount,
                            NumberingPattern = numberingPattern,
                            CreatedAt = DateTime.UtcNow
                        };
                        _context.NumberingPatternRows.Add(patternRow);
                    }
                }
            }

            // Update generated units if provided
            if (request.Request.GeneratedUnits?.Any() == true)
            {
                // Hard delete existing units for this project to avoid unique constraint issues
                var existingUnits = await _context.Units
                    .Where(u => u.Building.ProjectId == project.Id)
                    .ToListAsync(cancellationToken);

                if (existingUnits.Any())
                {
                    _context.Units.RemoveRange(existingUnits);
                    await _context.SaveChangesAsync(cancellationToken); // Save deletion first
                }

                // Get buildings and unit types for reference
                var buildings = await _context.Buildings
                    .Where(b => b.ProjectId == project.Id && !b.IsDeleted)
                    .ToListAsync(cancellationToken);
                var buildingDict = buildings.ToDictionary(b => b.Name, b => b);

                var unitTypes = await _context.UnitTypes
                    .Where(ut => ut.ProjectId == project.Id && !ut.IsDeleted)
                    .ToListAsync(cancellationToken);
                var unitTypeDict = unitTypes.ToDictionary(ut => ut.Name, ut => ut);

                // Add new generated units
                var unitsToAdd = new List<Domain.Entities.Unit>();
                foreach (var unitRequest in request.Request.GeneratedUnits)
                {
                    if (buildingDict.TryGetValue(unitRequest.BuildingName, out var building))
                    {
                        var unit = new Domain.Entities.Unit
                        {
                            UnitNumber = unitRequest.UnitNumber,
                            Floor = unitRequest.FloorNumber,
                            Position = unitRequest.Position,
                            BuildingId = building.Id,
                            UnitTypeId = !string.IsNullOrEmpty(unitRequest.UnitType) && unitTypeDict.TryGetValue(unitRequest.UnitType, out var unitType) 
                                ? unitType.Id 
                                : null,
                            CreatedAt = DateTime.UtcNow
                        };
                        unitsToAdd.Add(unit);
                    }
                }
                
                _context.Units.AddRange(unitsToAdd);
            }

            // Save changes
            await _context.SaveChangesAsync(cancellationToken);

            // Use CQRS to get the updated project data
            var getProjectQuery = new CRC.WebPortal.Application.Features.Projects.Queries.GetProject.GetProjectQuery(project.Id);
            var updatedProjectResult = await _mediator.Send(getProjectQuery, cancellationToken);

            if (updatedProjectResult.Succeeded && updatedProjectResult.Data != null)
            {
                return BaseResponse<CRC.WebPortal.Shared.Dtos.ProjectDto>.Success(updatedProjectResult.Data, "Project updated successfully.");
            }

            return BaseResponse<CRC.WebPortal.Shared.Dtos.ProjectDto>.Failure("Project updated but failed to retrieve updated data.");
        }
        catch (Exception ex)
        {
            return BaseResponse<CRC.WebPortal.Shared.Dtos.ProjectDto>.Failure($"Error updating project: {ex.Message}");
        }
    }
}
