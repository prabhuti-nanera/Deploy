using Microsoft.AspNetCore.Mvc;
using MediatR;
using CRC.WebPortal.Application.Features.UnitOwnerships.Commands.AssignOwnership;
using CRC.WebPortal.Application.Features.UnitOwnerships.Commands.UpdateOwnership;
using CRC.WebPortal.Application.Features.UnitOwnerships.Queries.GetOwnershipsByProject;
using CRC.WebPortal.Application.Features.UnitOwnerships.Queries.GetOwnership;
using CRC.WebPortal.Shared.Dtos;

namespace CRC.WebPortal.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UnitOwnershipController : ControllerBase
{
    private readonly IMediator _mediator;

    public UnitOwnershipController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// Assign ownership to a unit (Sell a unit)
    /// <param name="request">Ownership assignment details</param>
    /// <returns>Created ownership details</returns>
    [HttpPost("assign")]
    public async Task<IActionResult> AssignOwnership([FromBody] CreateUnitOwnershipDto request)
    {
        var command = new AssignOwnershipCommand(new AssignOwnershipRequest
        {
            OwnerName = request.OwnerName,
            OwnerEmail = request.OwnerEmail,
            OwnerMobile = request.OwnerMobile,
            SellingDetails = request.SellingDetails,
            SalePrice = request.SalePrice,
            SaleDate = request.SaleDate,
            SaleStatus = request.SaleStatus,
            UnitId = request.UnitId
        });

        var result = await _mediator.Send(command);

        if (result.Succeeded)
        {
            return CreatedAtAction(nameof(GetOwnership), new { id = result.Data!.Id }, result);
        }

        return BadRequest(result);
    }

    /// Update ownership details
    /// <param name="id">Ownership ID</param>
    /// <param name="request">Updated ownership details</param>
    /// <returns>Updated ownership details</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOwnership(int id, [FromBody] UpdateUnitOwnershipDto request)
    {
        if (id != request.Id)
        {
            return BadRequest("ID mismatch");
        }

        var command = new UpdateOwnershipCommand(new UpdateOwnershipRequest
        {
            Id = request.Id,
            OwnerName = request.OwnerName,
            OwnerEmail = request.OwnerEmail,
            OwnerMobile = request.OwnerMobile,
            SellingDetails = request.SellingDetails,
            SalePrice = request.SalePrice,
            SaleDate = request.SaleDate,
            SaleStatus = request.SaleStatus
        });

        var result = await _mediator.Send(command);

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    /// Get ownership details by ID
    /// <param name="id">Ownership ID</param>
    /// <returns>Ownership details</returns>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetOwnership(int id)
    {
        var query = new GetOwnershipQuery(id);
        var result = await _mediator.Send(query);

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return NotFound(result);
    }

    /// Get all ownerships for a project (Owners List)
    /// <param name="projectId">Project ID</param>
    /// <returns>List of unit ownerships</returns>
    [HttpGet("project/{projectId}")]
    public async Task<IActionResult> GetOwnershipsByProject(int projectId)
    {
        var query = new GetOwnershipsByProjectQuery(projectId);
        var result = await _mediator.Send(query);

        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    /// Get ownership summary for a project
    /// <param name="projectId">Project ID</param>
    /// <returns>Ownership summary statistics</returns>
    [HttpGet("project/{projectId}/summary")]
    public async Task<IActionResult> GetOwnershipSummary(int projectId)
    {
        var query = new GetOwnershipsByProjectQuery(projectId);
        var result = await _mediator.Send(query);

        if (result.Succeeded && result.Data != null)
        {
            var summary = new
            {
                TotalSoldUnits = result.Data.Count,
                TotalSaleValue = result.Data.Sum(o => o.SalePrice ?? 0),
                StatusBreakdown = result.Data.GroupBy(o => o.SaleStatus)
                    .Select(g => new { Status = g.Key, Count = g.Count() })
                    .ToList(),
                RecentSales = result.Data.OrderByDescending(o => o.SaleDate)
                    .Take(5)
                    .Select(o => new { o.UnitNumber, o.OwnerName, o.SaleDate, o.SalePrice })
                    .ToList()
            };

            return Ok(new { Succeeded = true, Data = summary, Message = "Ownership summary retrieved successfully." });
        }

        return BadRequest(result);
    }
}
