using CRC.WebPortal.Application.Features.Units.Commands.CreateUnit;
using CRC.WebPortal.Application.Features.Units.Commands.GenerateUnits;
using CRC.WebPortal.Application.Features.Units.Queries.GetUnitsByProject;
using CRC.WebPortal.Shared.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace CRC.WebPortal.API.Controllers;

[Authorize]
public class UnitsController : ApiControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateUnit([FromBody] CreateUnitRequest request)
    {
        var command = new CreateUnitCommand(request);
        var result = await Mediator.Send(command);
        return HandleResult(result);
    }

    [HttpPost("generate")]
    public async Task<IActionResult> GenerateUnits([FromBody] GenerateUnitsRequest request)
    {
        var command = new GenerateUnitsCommand(request);
        var result = await Mediator.Send(command);
        return HandleResult(result);
    }

    [HttpGet("project/{projectId}")]
    public async Task<IActionResult> GetUnitsByProject(int projectId)
    {
        var query = new GetUnitsByProjectQuery(projectId);
        var result = await Mediator.Send(query);
        return HandleResult(result);
    }
}
