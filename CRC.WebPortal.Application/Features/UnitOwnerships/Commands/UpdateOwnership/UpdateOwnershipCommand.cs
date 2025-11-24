using CRC.WebPortal.Application.Common.Dtos;
using CRC.Common.Models;
using MediatR;

namespace CRC.WebPortal.Application.Features.UnitOwnerships.Commands.UpdateOwnership;

public class UpdateOwnershipCommand : IRequest<BaseResponse<UnitOwnershipDto>>
{
    public UpdateOwnershipRequest Request { get; set; }

    public UpdateOwnershipCommand(UpdateOwnershipRequest request)
    {
        Request = request;
    }
}

public class UpdateOwnershipRequest
{
    public int Id { get; set; }
    public string OwnerName { get; set; } = string.Empty;
    public string OwnerEmail { get; set; } = string.Empty;
    public string OwnerMobile { get; set; } = string.Empty;
    public string? SellingDetails { get; set; }
    public decimal? SalePrice { get; set; }
    public DateTime? SaleDate { get; set; }
    public string SaleStatus { get; set; } = "Sold";
}
