using CRC.WebPortal.Application.Common.Dtos;
using CRC.Common.Models;
using MediatR;

namespace CRC.WebPortal.Application.Features.UnitOwnerships.Commands.AssignOwnership;

public class AssignOwnershipCommand : IRequest<BaseResponse<UnitOwnershipDto>>
{
    public AssignOwnershipRequest Request { get; set; }

    public AssignOwnershipCommand(AssignOwnershipRequest request)
    {
        Request = request;
    }
}

public class AssignOwnershipRequest
{
    public string OwnerName { get; set; } = string.Empty;
    public string OwnerEmail { get; set; } = string.Empty;
    public string OwnerMobile { get; set; } = string.Empty;
    public string? SellingDetails { get; set; }
    public decimal? SalePrice { get; set; }
    public DateTime? SaleDate { get; set; }
    public string SaleStatus { get; set; } = "Sold";
    public int UnitId { get; set; }
}
