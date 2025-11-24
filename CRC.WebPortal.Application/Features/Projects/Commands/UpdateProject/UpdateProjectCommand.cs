using CRC.WebPortal.Application.Common.Dtos;
using CRC.Common.Models;
using MediatR;

namespace CRC.WebPortal.Application.Features.Projects.Commands.UpdateProject;

public record UpdateProjectCommand(CRC.WebPortal.Shared.Requests.UpdateProjectRequest Request) : IRequest<BaseResponse<CRC.WebPortal.Shared.Dtos.ProjectDto>>;
