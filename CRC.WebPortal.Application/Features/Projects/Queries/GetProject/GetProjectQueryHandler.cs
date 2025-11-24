using CRC.WebPortal.Application.Common.Dtos;
using CRC.WebPortal.Domain.Entities;
using CRC.WebPortal.Domain.Interfaces;
using CRC.Common.Models;
using CRC.WebPortal.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using MediatR;
using CRC.WebPortal.Shared.Dtos;
using CRC.WebPortal.Application.Features.UnitTypes.Queries.GetUnitTypesByProject;
using CRC.WebPortal.Application.Features.NumberingPatterns.Queries.GetNumberingPatternsByProject;
using CRC.WebPortal.Application.Features.Units.Queries.GetUnitsByProject;

namespace CRC.WebPortal.Application.Features.Projects.Queries.GetProject;

public class GetProjectQueryHandler : IRequestHandler<GetProjectQuery, BaseResponse<CRC.WebPortal.Shared.Dtos.ProjectDto>>
{
    private readonly ApplicationDbContext _context;
    private readonly IMediator _mediator;

    public GetProjectQueryHandler(ApplicationDbContext context, IMediator mediator)
    {
        _context = context;
        _mediator = mediator;
    }


    public async Task<BaseResponse<CRC.WebPortal.Shared.Dtos.ProjectDto>> Handle(GetProjectQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Get basic project info (without includes)
            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

            if (project == null)
                return BaseResponse<CRC.WebPortal.Shared.Dtos.ProjectDto>.Failure("Project not found.");

            // Get buildings for this project (exclude deleted ones)
            var buildings = await _context.Buildings
                .Where(b => b.ProjectId == project.Id && !b.IsDeleted)
                .ToListAsync(cancellationToken);

            // Use dedicated CQRS queries to get related data
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

            // Convert to DTO with complete data
            var projectDto = new CRC.WebPortal.Shared.Dtos.ProjectDto
            {
                Id = project.Id,
                Name = project.Name,
                Address = project.Address,
                Type = project.Type,
                AdminName = project.AdminName,
                AdminMobile = project.AdminMobile,
                CreatedAt = project.CreatedAt,
                TotalBuildings = buildings.Count,
                TotalUnits = units.Count,
                Buildings = buildings.Select(b => new CRC.WebPortal.Shared.Dtos.BuildingDto
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

            return BaseResponse<CRC.WebPortal.Shared.Dtos.ProjectDto>.Success(projectDto, "Project retrieved successfully.");
        }
        catch (Exception ex)
        {
            return BaseResponse<CRC.WebPortal.Shared.Dtos.ProjectDto>.Failure($"Error retrieving project: {ex.Message}");
        }
    }
}
