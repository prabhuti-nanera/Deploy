using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using CRC.Common.Models;

namespace CRC.WebPortal.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class ApiControllerBase : ControllerBase
    {
        private ISender? _mediator;

        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

        protected IActionResult HandleResult<T>(BaseResponse<T> result)
        {
            if (result == null) return NotFound();
            if (result.Succeeded) return Ok(result);
            return BadRequest(result);
        }

        protected IActionResult HandleResult(BaseResponse result)
        {
            if (result == null) return NotFound();
            if (result.Succeeded) return Ok(result);
            return BadRequest(result);
        }
    }
}
