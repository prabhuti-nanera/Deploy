using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using CRC.WebPortal.Application.Features.Projects.Commands.CreateProject;
using CRC.WebPortal.Application.Features.Projects.Commands.UpdateProject;
using CRC.WebPortal.Application.Features.Projects.Commands.DeleteProject;
using CRC.WebPortal.Application.Features.Projects.Queries.GetProjects;
using CRC.WebPortal.Application.Features.Projects.Queries.GetProject;
using CRC.WebPortal.Application.Common.Dtos;
using CRC.Common.Models;

namespace CRC.WebPortal.API.Controllers;

// [Authorize] // Temporarily disabled for testing
public class ProjectController : ApiControllerBase
{
    /// Get all projects
    [HttpGet]
    public async Task<IActionResult> GetProjects()
    {
        var query = new GetProjectsQuery();
        var result = await Mediator.Send(query);
        
        return result.Succeeded ? Ok(result) : BadRequest(result);
    }

    /// Get project by ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetProject(int id)
    {
        var query = new GetProjectQuery(id);
        var result = await Mediator.Send(query);
        
        return result.Succeeded ? Ok(result) : BadRequest(result);
    }
    /// Test endpoint with dummy data
    [HttpPost("test")]
    public IActionResult TestCreateProject()
    {
        var dummyProject = new ProjectDto
        {
            Id = 1,
            Name = "Test Project",
            Type = "Res",
            Address = "123 Test Street",
            AdminName = "Test Admin",
            AdminMobile = "1234567890",
            TotalBuildings = 1,
            TotalUnits = 10,
            CreatedAt = DateTime.Now,
            Buildings = new List<BuildingDto>()
        };
        
        var response = BaseResponse<ProjectDto>.Success(dummyProject, "Test project created successfully.");
        return Ok(response);
    }

    /// Create a new project
    [HttpPost]
    public async Task<IActionResult> CreateProject([FromBody] CreateProjectRequest request)
    {
        var command = new CreateProjectCommand(request);
        var result = await Mediator.Send(command);
        
        if (result.Succeeded)
        {
            return CreatedAtAction(nameof(GetProject), new { id = result.Data?.Id }, result);
        }
        
        return BadRequest(result);
    }

    /// Update an existing project
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProject(int id, [FromBody] CRC.WebPortal.Shared.Requests.UpdateProjectRequest request)
    {
        if (request == null)
        {
            return BadRequest("Request cannot be null.");
        }
        
        if (id != request.Id)
        {
            return BadRequest("Project ID mismatch.");
        }

        var command = new UpdateProjectCommand(request);
        var result = await Mediator.Send(command);
        
        return result.Succeeded ? Ok(result) : BadRequest(result);
    }

    /// Delete a project
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProject(int id)
    {
        var command = new DeleteProjectCommand(id);
        var result = await Mediator.Send(command);
        
        return result.Succeeded ? Ok(result) : BadRequest(result);
    }
}
