using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CRC.WebPortal.Application.Features.UnitTypes.Commands.CreateUnitType;
using CRC.WebPortal.Application.Features.UnitTypes.Commands.UpdateUnitType;
using CRC.WebPortal.Application.Features.UnitTypes.Commands.DeleteUnitType;
using CRC.WebPortal.Application.Features.UnitTypes.Commands.CloneUnitType;
using CRC.WebPortal.Application.Features.UnitTypes.Queries.GetUnitTypesByProject;
using CRC.WebPortal.Application.Features.UnitTypes.Queries.GetUnitTypeById;
using CRC.WebPortal.Application.Common.Dtos;

namespace CRC.WebPortal.API.Controllers;

[Authorize]
public class UnitTypeController : ApiControllerBase
{
    /// Get all unit types for a project
    [HttpGet("project/{projectId}")]
    public async Task<IActionResult> GetUnitTypesByProject(int projectId)
    {
        var query = new GetUnitTypesByProjectQuery(projectId);
        var result = await Mediator.Send(query);
        
        return HandleResult(result);
    }

    /// Get unit type by ID
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUnitType(int id)
    {
        var query = new GetUnitTypeByIdQuery(id);
        var result = await Mediator.Send(query);
        
        return HandleResult(result);
    }

    /// Create a new unit type
    [HttpPost]
    public async Task<IActionResult> CreateUnitType([FromBody] CreateUnitTypeRequest request)
    {
        var command = new CreateUnitTypeCommand(request);
        var result = await Mediator.Send(command);
        
        if (result.Succeeded)
        {
            return CreatedAtAction(nameof(GetUnitType), new { id = result.Data?.Id }, result);
        }
        
        return HandleResult(result);
    }
    /// Update an existing unit type
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUnitType(int id, [FromBody] UpdateUnitTypeRequest request)
    {
        if (id != request.Id)
        {
            return BadRequest("Unit type ID mismatch.");
        }

        var command = new UpdateUnitTypeCommand(request);
        var result = await Mediator.Send(command);
        
        return HandleResult(result);
    }

    /// Delete a unit type
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUnitType(int id)
    {
        var command = new DeleteUnitTypeCommand(id);
        var result = await Mediator.Send(command);
        
        return HandleResult(result);
    }

    /// Clone an existing unit type

    [HttpPost("clone")]
    public async Task<IActionResult> CloneUnitType([FromBody] CloneUnitTypeRequest request)
    {
        var command = new CloneUnitTypeCommand(request);
        var result = await Mediator.Send(command);
        
        if (result.Succeeded)
        {
            return CreatedAtAction(nameof(GetUnitType), new { id = result.Data?.Id }, result);
        }
        
        return HandleResult(result);
    }
}
