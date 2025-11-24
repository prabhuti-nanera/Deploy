using CRC.WebPortal.Infrastructure.Data;
using CRC.Common.Models;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace CRC.WebPortal.Application.Features.UnitOwnerships.Queries.GetOwnershipsByProject;

public class GetOwnershipsByProjectQueryHandler : IRequestHandler<GetOwnershipsByProjectQuery, BaseResponse<List<CRC.WebPortal.Shared.Dtos.UnitOwnershipDto>>>
{
    private readonly ApplicationDbContext _context;

    public GetOwnershipsByProjectQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<BaseResponse<List<CRC.WebPortal.Shared.Dtos.UnitOwnershipDto>>> Handle(GetOwnershipsByProjectQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var ownerships = await _context.Set<Domain.Entities.UnitOwnership>()
                .Include(o => o.Unit)
                    .ThenInclude(u => u.Building)
                .Include(o => o.Unit)
                    .ThenInclude(u => u.UnitType)
                .Where(o => o.Unit.Building.ProjectId == request.ProjectId && !o.IsDeleted)
                .OrderBy(o => o.Unit.Building.Name)
                .ThenBy(o => o.Unit.UnitNumber)
                .Select(o => new CRC.WebPortal.Shared.Dtos.UnitOwnershipDto
                {
                    Id = o.Id,
                    OwnerName = o.OwnerName,
                    OwnerEmail = o.OwnerEmail,
                    OwnerMobile = o.OwnerMobile,
                    SellingDetails = o.SellingDetails,
                    SalePrice = o.SalePrice,
                    SaleDate = o.SaleDate,
                    SaleStatus = o.SaleStatus,
                    UnitId = o.UnitId,
                    UnitNumber = o.Unit.UnitNumber,
                    BuildingName = o.Unit.Building.Name,
                    UnitType = o.Unit.UnitType != null ? o.Unit.UnitType.Name : null,
                    CreatedAt = o.CreatedAt,
                    UpdatedAt = o.UpdatedAt
                })
                .ToListAsync(cancellationToken);

            return BaseResponse<List<CRC.WebPortal.Shared.Dtos.UnitOwnershipDto>>.Success(ownerships, "Unit ownerships retrieved successfully.");
        }
        catch (Exception ex)
        {
            return BaseResponse<List<CRC.WebPortal.Shared.Dtos.UnitOwnershipDto>>.Failure($"Error retrieving unit ownerships: {ex.Message}");
        }
    }
}
