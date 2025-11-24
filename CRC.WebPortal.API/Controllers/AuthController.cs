using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using CRC.Common.Models;
using CRC.WebPortal.Shared.Dtos;
using CRC.WebPortal.Shared.Requests;
using CRC.WebPortal.Application.Features.Auth.Commands.SignUp;
using CRC.WebPortal.Application.Features.Auth.Commands.SignIn;
using CRC.WebPortal.Application.Features.Auth.Commands.SendOtp;
using CRC.WebPortal.Application.Features.Auth.Commands.VerifyOtp;
using CRC.WebPortal.Domain.Interfaces;
using MediatR;

namespace CRC.WebPortal.API.Controllers;
    
/// Auth Controller with Clean Architecture and CQRS patterns
/// - UI sends simple data structures (DTOs) 
/// - API internally creates and executes appropriate CQRS commands
/// - UI doesn't know about internal command structure (encapsulation)

[ApiController]
[Route("api/[controller]")]
public class AuthController : ApiControllerBase
{
    private readonly IMapper _mapper;

    public AuthController(IMapper mapper)
    {
        _mapper = mapper;
    }

    [HttpPost("signup")]
    [ProducesResponseType(typeof(AuthResponse), 200)]
    [ProducesResponseType(typeof(AuthResponse), 400)]
    public async Task<IActionResult> SignUp([FromBody] SignupRequest request)
    {
        var command = _mapper.Map<SignUpCommand>(request);
        var result = await Mediator.Send(command);
        
        if (result.Succeeded)
        {
            return Ok(result);
        }
        
        return BadRequest(result);
    }

    [HttpPost("signin")]
    [ProducesResponseType(typeof(AuthResponse), 200)]
    [ProducesResponseType(typeof(AuthResponse), 400)]
    public async Task<IActionResult> SignIn([FromBody] SigninRequest request)
    {
        var command = _mapper.Map<SignInCommand>(request);
        var result = await Mediator.Send(command);
        
        if (result.Succeeded)
        {
            return Ok(result);
        }
        
        return BadRequest(result);
    }

    [HttpPost("send-otp")]
    [ProducesResponseType(typeof(AuthResponse), 200)]
    [ProducesResponseType(typeof(AuthResponse), 400)]
    public async Task<IActionResult> SendOtp([FromBody] SendOtpRequest request)
    {
        var command = _mapper.Map<SendOtpCommand>(request);
        var result = await Mediator.Send(command);
        
        if (result.Succeeded)
        {
            return Ok(result);
        }
        
        return BadRequest(result);
    }

    [HttpPost("verify-otp")]
    [ProducesResponseType(typeof(AuthResponse), 200)]
    [ProducesResponseType(typeof(AuthResponse), 400)]
    public async Task<IActionResult> VerifyOtp([FromBody] CRC.Common.Models.VerifyOtpRequest request)
    {
        var command = _mapper.Map<VerifyOtpCommand>(request);
        var result = await Mediator.Send(command);
        
        if (result.Succeeded)
        {
            return Ok(result);
        }
        
        return BadRequest(result);
    }

}
