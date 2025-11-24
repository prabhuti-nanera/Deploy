using CRC.WebPortal.Application.Common.Models;
using MediatR;

namespace CRC.WebPortal.Application.Features.Auth.Commands.VerifyOtp;

public class VerifyOtpCommand : IRequest<AuthResponse>
{
    public string Email { get; set; } = string.Empty;
    public string OtpCode { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}
