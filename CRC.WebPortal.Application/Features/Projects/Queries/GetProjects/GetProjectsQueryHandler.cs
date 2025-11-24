using CRC.WebPortal.Application.Common.Dtos;
using CRC.WebPortal.Domain.Entities;
using CRC.WebPortal.Domain.Interfaces;
using CRC.Common.Models;
using CRC.WebPortal.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using MediatR;
using CRC.WebPortal.Application.Common;

namespace CRC.WebPortal.Application.Features.Projects.Queries.GetProjects;

public class GetProjectsQueryHandler : IRequestHandler<GetProjectsQuery, BaseResponse<List<ProjectDto>>>
{
    private readonly ApplicationDbContext _context;

    public GetProjectsQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<BaseResponse<List<ProjectDto>>> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Try database first, fallback to in-memory if it fails
            var projects = await _context.Projects
                .Include(p => p.Buildings)
                .Include(p => p.UnitTypes)
                .ToListAsync(cancellationToken);
            
            var projectDtos = projects.Select(p => new ProjectDto
            {
                Id = p.Id,
                Name = p.Name,
                Address = p.Address,
                Type = p.Type,
                AdminName = p.AdminName,
                AdminMobile = p.AdminMobile,
                CreatedAt = p.CreatedAt,
                TotalBuildings = p.Buildings.Count,
                TotalUnits = p.Buildings.Sum(b => b.Floors * b.UnitsPerFloor),
                Buildings = p.Buildings.Select(b => new BuildingDto
                {
                    Id = b.Id,
                    Name = b.Name,
                    Type = b.Type,
                    Floors = b.Floors,
                    UnitsPerFloor = b.UnitsPerFloor,
                    TotalUnits = b.Floors * b.UnitsPerFloor,
                    ProjectId = b.ProjectId
                }).ToList(),
                UnitTypes = p.UnitTypes.Select(ut => ut.Name).ToList()
            }).ToList();
            
            return BaseResponse<List<ProjectDto>>.Success(projectDtos, "Projects retrieved successfully.");
        }
        catch (Exception ex)
        {
            return BaseResponse<List<ProjectDto>>.Failure($"Error retrieving projects: {ex.Message}");
        }
    }

    // Helper class for raw SQL results
    public class ProjectRawDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string AdminName { get; set; } = string.Empty;
        public string AdminMobile { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
