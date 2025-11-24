using CRC.WebPortal.Application.Common.Dtos;
using CRC.Common.Models;
using MediatR;

namespace CRC.WebPortal.Application.Features.Projects.Queries.GetProject;

public record GetProjectQuery(int Id) : IRequest<BaseResponse<CRC.WebPortal.Shared.Dtos.ProjectDto>>;
