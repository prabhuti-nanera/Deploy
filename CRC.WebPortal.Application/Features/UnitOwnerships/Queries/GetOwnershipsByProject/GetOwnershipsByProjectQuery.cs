using CRC.Common.Models;
using MediatR;

namespace CRC.WebPortal.Application.Features.UnitOwnerships.Queries.GetOwnershipsByProject;

public class GetOwnershipsByProjectQuery : IRequest<BaseResponse<List<CRC.WebPortal.Shared.Dtos.UnitOwnershipDto>>>
{
    public int ProjectId { get; set; }

    public GetOwnershipsByProjectQuery(int projectId)
    {
        ProjectId = projectId;
    }
}
