using CRC.WebPortal.Application.Common.Models;
using MediatR;

namespace CRC.WebPortal.Application.Features.Auth.Commands.SignUp;

public class SignUpCommand : IRequest<AuthResponse>
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}
