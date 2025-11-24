using CRC.WebPortal.Domain.Entities;
using CRC.WebPortal.Domain.Interfaces;
using CRC.Common.Models;
using MediatR;

namespace CRC.WebPortal.Application.Features.Projects.Commands.DeleteProject;

public class DeleteProjectCommandHandler : IRequestHandler<DeleteProjectCommand, BaseResponse<bool>>
{
    private readonly IRepository<Project> _projectRepository;

    public DeleteProjectCommandHandler(IRepository<Project> projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<BaseResponse<bool>> Handle(DeleteProjectCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var project = await _projectRepository.GetByIdAsync(request.Id);
            if (project == null)
            {
                return BaseResponse<bool>.Failure("Project not found.");
            }

            // For now, just delete the project (hard delete)
            _projectRepository.Delete(project);
            await _projectRepository.SaveChangesAsync();

            return BaseResponse<bool>.Success(true, "Project deleted successfully.");
        }
        catch (Exception ex)
        {
            return BaseResponse<bool>.Failure($"Error deleting project: {ex.Message}");
        }
    }
}
