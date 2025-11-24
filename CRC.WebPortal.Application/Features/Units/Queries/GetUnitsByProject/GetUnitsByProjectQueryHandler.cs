using CRC.WebPortal.Domain.Entities;
using CRC.WebPortal.Domain.Interfaces;
using CRC.Common.Models;
using CRC.WebPortal.Shared.Dtos;
using CRC.WebPortal.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace CRC.WebPortal.Application.Features.Units.Queries.GetUnitsByProject;

public class GetUnitsByProjectQueryHandler : IRequestHandler<GetUnitsByProjectQuery, BaseResponse<List<UnitDto>>>
{
    private readonly ApplicationDbContext _context;

    public GetUnitsByProjectQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<BaseResponse<List<UnitDto>>> Handle(GetUnitsByProjectQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Get all units for the project with related data
            var units = await _context.Units
                .Include(u => u.Building)
                .Include(u => u.UnitType)
                .Include(u => u.Ownership)
                .Where(u => u.Building.ProjectId == request.ProjectId && !u.IsDeleted)
                .OrderBy(u => u.Building.Name)
                .ThenBy(u => u.Floor)
                .ThenBy(u => u.UnitNumber)
                .Select(u => new UnitDto
                {
                    Id = u.Id,
                    UnitNumber = u.UnitNumber,
                    UnitType = u.UnitType != null ? u.UnitType.Name : null,
                    FloorNumber = u.Floor,
                    BuildingName = u.Building.Name,
                    ProjectId = request.ProjectId,
                    Position = u.Position,
                    BuildingId = u.BuildingId,
                    OwnershipStatus = u.OwnershipStatus,
                    Ownership = u.Ownership != null ? new UnitOwnershipDto
                    {
                        Id = u.Ownership.Id,
                        OwnerName = u.Ownership.OwnerName,
                        OwnerEmail = u.Ownership.OwnerEmail,
                        OwnerMobile = u.Ownership.OwnerMobile,
                        SellingDetails = u.Ownership.SellingDetails,
                        SalePrice = u.Ownership.SalePrice,
                        SaleDate = u.Ownership.SaleDate,
                        SaleStatus = u.Ownership.SaleStatus,
                        UnitId = u.Ownership.UnitId,
                        UnitNumber = u.UnitNumber,
                        BuildingName = u.Building.Name,
                        UnitType = u.UnitType != null ? u.UnitType.Name : null,
                        CreatedAt = u.Ownership.CreatedAt,
                        UpdatedAt = u.Ownership.UpdatedAt
                    } : null,
                    CreatedAt = u.CreatedAt
                })
                .ToListAsync(cancellationToken);

            return BaseResponse<List<UnitDto>>.Success(units, "Units retrieved successfully.");
        }
        catch (Exception ex)
        {
            return BaseResponse<List<UnitDto>>.Failure($"Error retrieving units: {ex.Message}");
        }
    }
}
