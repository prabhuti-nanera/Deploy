using CRC.WebPortal.Application.Common.Dtos;
using CRC.Common.Models;
using MediatR;

namespace CRC.WebPortal.Application.Features.Projects.Commands.CreateProject;

public record CreateProjectCommand(CreateProjectRequest Request) : IRequest<BaseResponse<CRC.WebPortal.Shared.Dtos.ProjectDto>>;
