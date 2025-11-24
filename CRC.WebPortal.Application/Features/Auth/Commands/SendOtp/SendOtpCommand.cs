using CRC.WebPortal.Application.Common.Models;
using MediatR;

namespace CRC.WebPortal.Application.Features.Auth.Commands.SendOtp;

public class SendOtpCommand : IRequest<AuthResponse>
{
    public string Email { get; set; } = string.Empty;
}
