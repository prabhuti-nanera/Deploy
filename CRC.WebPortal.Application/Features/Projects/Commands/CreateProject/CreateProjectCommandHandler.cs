using CRC.Common.Models;
using CRC.WebPortal.Application.Common.Dtos;
using CRC.WebPortal.Domain.Entities;
using CRC.WebPortal.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MediatR;
using CRC.WebPortal.Shared.Dtos;
using CRC.WebPortal.Application.Features.UnitTypes.Queries.GetUnitTypesByProject;
using CRC.WebPortal.Application.Features.NumberingPatterns.Queries.GetNumberingPatternsByProject;
using CRC.WebPortal.Application.Features.Units.Queries.GetUnitsByProject;

namespace CRC.WebPortal.Application.Features.Projects.Commands.CreateProject;

public class CreateProjectCommandHandler : IRequestHandler<CreateProjectCommand, BaseResponse<CRC.WebPortal.Shared.Dtos.ProjectDto>>
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<CreateProjectCommandHandler> _logger;
    private readonly IMediator _mediator;

    public CreateProjectCommandHandler(ApplicationDbContext context, ILogger<CreateProjectCommandHandler> logger, IMediator mediator)
    {
        _context = context;
        _logger = logger;
        _mediator = mediator;
    }


    public async Task<BaseResponse<CRC.WebPortal.Shared.Dtos.ProjectDto>> Handle(CreateProjectCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Create project entity
            var project = new Project
            {
                Name = request.Request.Name,
                Address = request.Request.Address,
                Type = request.Request.Type,
                AdminName = request.Request.AdminName,
                AdminMobile = request.Request.AdminMobile,
                CreatedAt = DateTime.UtcNow
            };

            _context.Projects.Add(project);
            await _context.SaveChangesAsync(cancellationToken);

            // Add unit types if provided
            if (request.Request.UnitTypes?.Any() == true)
            {
                for (int i = 0; i < request.Request.UnitTypes.Count; i++)
                {
                    var unitTypeRequest = request.Request.UnitTypes[i];
                    var unitType = new UnitType
                    {
                        Name = unitTypeRequest.Name,
                        Description = unitTypeRequest.Description ?? string.Empty,
                        DisplayOrder = unitTypeRequest.DisplayOrder,
                        ProjectId = project.Id,
                        CreatedAt = DateTime.UtcNow
                    };
                    _context.UnitTypes.Add(unitType);
                }
            }

            // Add buildings if provided
            if (request.Request.Buildings?.Any() == true)
            {
                foreach (var buildingRequest in request.Request.Buildings)
                {
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

            // Save changes to ensure buildings and unit types are in the database before generating units
            await _context.SaveChangesAsync(cancellationToken);

            // Add numbering pattern if provided
            if (request.Request.NumberingPattern != null)
            {
                var numberingPattern = new NumberingPattern
                {
                    StartFloorNumber = request.Request.NumberingPattern.StartFloorNumber,
                    UnitNameDigits = request.Request.NumberingPattern.UnitNameDigits,
                    ApplyTo = request.Request.NumberingPattern.ApplyTo,
                    ProjectId = project.Id,
                    CreatedAt = DateTime.UtcNow
                };
                _context.NumberingPatterns.Add(numberingPattern);
                await _context.SaveChangesAsync(cancellationToken);

                // Add pattern rows if provided
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
                            OriginalDigitCount = rowRequest.OriginalDigitCount,
                            DigitsJson = rowRequest.Digits?.Any() == true ? System.Text.Json.JsonSerializer.Serialize(rowRequest.Digits) : "[]",
                            CustomDigitsJson = rowRequest.CustomDigits?.Any() == true ? System.Text.Json.JsonSerializer.Serialize(rowRequest.CustomDigits) : "[]",
                            NumberingPatternId = numberingPattern.Id,
                            CreatedAt = DateTime.UtcNow
                        };
                        _context.NumberingPatternRows.Add(patternRow);
                    }
                }
            }

            // Generate units automatically based on building configuration
            // Add generated units if provided OR generate them automatically from buildings
            if (request.Request.GeneratedUnits?.Any() == true || request.Request.Buildings?.Any() == true)
            {
                // First, get the building and unit type mappings (exclude deleted ones)
                var buildings = await _context.Buildings
                    .Where(b => b.ProjectId == project.Id && !b.IsDeleted)
                    .ToListAsync(cancellationToken);
                
                var projectUnitTypes = await _context.UnitTypes
                    .Where(ut => ut.ProjectId == project.Id)
                    .ToListAsync(cancellationToken);

                // Generate units from provided data or auto-generate from buildings
                if (request.Request.GeneratedUnits?.Any() == true)
                {
                    // Use provided generated units
                    foreach (var unitRequest in request.Request.GeneratedUnits)
                    {
                        // Find the building by name (frontend sends BuildingName, not BuildingId)
                        var building = buildings.FirstOrDefault(b => b.Name == unitRequest.BuildingName);
                        
                        // Find the unit type by name (frontend sends UnitType name, not UnitTypeId)
                        var unitType = projectUnitTypes.FirstOrDefault(ut => ut.Name == unitRequest.UnitType);

                        // Only create unit if building is found
                        if (building != null)
                        {
                            var unit = new CRC.WebPortal.Domain.Entities.Unit
                            {
                                UnitNumber = unitRequest.UnitNumber,
                                Floor = unitRequest.Floor,
                                Position = 1, // Default position since CreateUnitRequest might not have Position
                                BuildingId = building.Id,
                                UnitTypeId = unitType?.Id, // Use found unit type ID or null
                                CreatedAt = DateTime.UtcNow
                            };
                            _context.Units.Add(unit);
                        }
                    }
                }
                else
                {
                    // Auto-generate units from building configuration
                    var defaultUnitType = projectUnitTypes.FirstOrDefault();
                    
                    foreach (var building in buildings)
                    {
                        for (int floor = 1; floor <= building.Floors; floor++)
                        {
                            for (int position = 1; position <= building.UnitsPerFloor; position++)
                            {
                                var unitNumber = $"{floor}{position:D2}"; // e.g., "101", "102", "201", etc.
                                
                                var unit = new CRC.WebPortal.Domain.Entities.Unit
                                {
                                    UnitNumber = unitNumber,
                                    Floor = floor,
                                    Position = position,
                                    BuildingId = building.Id,
                                    UnitTypeId = defaultUnitType?.Id,
                                    CreatedAt = DateTime.UtcNow
                                };
                                _context.Units.Add(unit);
                            }
                        }
                    }
                }
            }

            await _context.SaveChangesAsync(cancellationToken);

            // Load the complete project with all related data using CQRS queries
            var savedProject = await _context.Projects
                .FirstOrDefaultAsync(p => p.Id == project.Id, cancellationToken);
                
            // Get buildings for this project (exclude deleted ones)
            var projectBuildings = await _context.Buildings
                .Where(b => b.ProjectId == project.Id && !b.IsDeleted)
                .ToListAsync(cancellationToken);

            // Use dedicated CQRS queries to get related data (same as GetProjectQueryHandler)
            var unitTypesResponse = await _mediator.Send(new GetUnitTypesByProjectQuery(project.Id), cancellationToken);
            var numberingPatternsResponse = await _mediator.Send(new GetNumberingPatternsByProjectQuery(project.Id), cancellationToken);
            var unitsResponse = await _mediator.Send(new GetUnitsByProjectQuery(project.Id), cancellationToken);

            // Handle potential failures from sub-queries and convert to Shared DTOs
            var unitTypes = unitTypesResponse.Succeeded && unitTypesResponse.Data != null 
                ? unitTypesResponse.Data.Select(ut => new CRC.WebPortal.Shared.Dtos.UnitTypeDto
                {
                    Id = ut.Id,
                    Name = ut.Name,
                    Description = ut.Description ?? "",
                    DisplayOrder = ut.DisplayOrder,
                    ProjectId = ut.ProjectId,
                    IsActive = ut.IsActive,
                    CreatedAt = ut.CreatedAt
                }).ToList()
                : new List<CRC.WebPortal.Shared.Dtos.UnitTypeDto>();
            
            var numberingPatterns = numberingPatternsResponse.Succeeded && numberingPatternsResponse.Data != null ? numberingPatternsResponse.Data : new List<UnitNumberingPatternDto>();
            var units = unitsResponse.Succeeded && unitsResponse.Data != null ? unitsResponse.Data : new List<CRC.WebPortal.Shared.Dtos.UnitDto>();

            // Group units by building for building DTOs
            var unitsByBuilding = units.GroupBy(u => u.BuildingName).ToDictionary(g => g.Key, g => g.ToList());

            // Convert to Shared DTO (same as GetProjectQueryHandler)
            var projectDto = new CRC.WebPortal.Shared.Dtos.ProjectDto
            {
                Id = savedProject!.Id,
                Name = savedProject.Name,
                Address = savedProject.Address,
                Type = savedProject.Type,
                AdminName = savedProject.AdminName,
                AdminMobile = savedProject.AdminMobile,
                CreatedAt = savedProject.CreatedAt,
                TotalBuildings = projectBuildings.Count,
                TotalUnits = units.Count,
                Buildings = projectBuildings.Select(b => new CRC.WebPortal.Shared.Dtos.BuildingDto
                {
                    Id = b.Id,
                    Name = b.Name,
                    Type = b.Type,
                    Floors = b.Floors,
                    UnitsPerFloor = b.UnitsPerFloor,
                    TotalUnits = unitsByBuilding.TryGetValue(b.Name, out var buildingUnits) ? buildingUnits.Count : 0,
                    ProjectId = b.ProjectId,
                    Units = unitsByBuilding.TryGetValue(b.Name, out var buildingUnitsForDto) ? buildingUnitsForDto : new List<CRC.WebPortal.Shared.Dtos.UnitDto>()
                }).ToList(),
                UnitTypes = unitTypes,
                GeneratedUnits = units,
                NumberingPattern = numberingPatterns.FirstOrDefault()
            };

            return BaseResponse<CRC.WebPortal.Shared.Dtos.ProjectDto>.Success(projectDto, "Project created successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating project: {Message}", ex.Message);
            
            // Check for unique constraint violation
            if (ex.Message.Contains("duplicate key value violates unique constraint") && ex.Message.Contains("IX_Projects_Name"))
            {
                return BaseResponse<CRC.WebPortal.Shared.Dtos.ProjectDto>.Failure($"A project with the name '{request.Request.Name}' already exists. Please choose a different name.");
            }
            
            return BaseResponse<CRC.WebPortal.Shared.Dtos.ProjectDto>.Failure($"Error creating project: {ex.Message}");
        }
    }

}
