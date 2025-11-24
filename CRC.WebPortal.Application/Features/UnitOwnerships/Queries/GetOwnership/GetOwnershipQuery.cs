using CRC.Common.Models;
using MediatR;

namespace CRC.WebPortal.Application.Features.UnitOwnerships.Queries.GetOwnership;

public class GetOwnershipQuery : IRequest<BaseResponse<CRC.WebPortal.Shared.Dtos.UnitOwnershipDto>>
{
    public int Id { get; set; }

    public GetOwnershipQuery(int id)
    {
        Id = id;
    }
}
