using CRC.Common.Models;
using MediatR;

namespace CRC.WebPortal.Application.Features.Projects.Commands.DeleteProject;

public record DeleteProjectCommand(int Id) : IRequest<BaseResponse<bool>>;
