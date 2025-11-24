using CRC.WebPortal.Infrastructure.Data;
using CRC.Common.Models;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace CRC.WebPortal.Application.Features.UnitOwnerships.Queries.GetOwnership;

public class GetOwnershipQueryHandler : IRequestHandler<GetOwnershipQuery, BaseResponse<CRC.WebPortal.Shared.Dtos.UnitOwnershipDto>>
{
    private readonly ApplicationDbContext _context;

    public GetOwnershipQueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<BaseResponse<CRC.WebPortal.Shared.Dtos.UnitOwnershipDto>> Handle(GetOwnershipQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var ownership = await _context.Set<Domain.Entities.UnitOwnership>()
                .Include(o => o.Unit)
                    .ThenInclude(u => u.Building)
                .Include(o => o.Unit)
                    .ThenInclude(u => u.UnitType)
                .Where(o => o.Id == request.Id && !o.IsDeleted)
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
                .FirstOrDefaultAsync(cancellationToken);

            if (ownership == null)
            {
                return BaseResponse<CRC.WebPortal.Shared.Dtos.UnitOwnershipDto>.Failure("Unit ownership not found.");
            }

            return BaseResponse<CRC.WebPortal.Shared.Dtos.UnitOwnershipDto>.Success(ownership, "Unit ownership retrieved successfully.");
        }
        catch (Exception ex)
        {
            return BaseResponse<CRC.WebPortal.Shared.Dtos.UnitOwnershipDto>.Failure($"Error retrieving unit ownership: {ex.Message}");
        }
    }
}
