using CRC.WebPortal.Application.Common.Dtos;
using CRC.Common.Models;
using MediatR;

namespace CRC.WebPortal.Application.Features.Projects.Queries.GetProjects;

public record GetProjectsQuery : IRequest<BaseResponse<List<ProjectDto>>>;
