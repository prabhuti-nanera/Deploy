using AutoMapper;
using CRC.WebPortal.Application.Common.Models;
using CRC.WebPortal.Application.Common;
using CRC.WebPortal.Domain.Entities;
using CRC.WebPortal.Domain.Interfaces;
using MediatR;

namespace CRC.WebPortal.Application.Features.Auth.Commands.SignIn;

public class SignInCommandHandler : IRequestHandler<SignInCommand, AuthResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;

    public SignInCommandHandler(IUnitOfWork unitOfWork, ITokenService tokenService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
        _mapper = mapper;
    }

    public async Task<AuthResponse> Handle(SignInCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var user = await _unitOfWork.Users.GetByEmailAsync(request.Email.ToLowerInvariant());
            
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return new AuthResponse 
                { 
                    Succeeded = false,
                    Message = "Invalid email or password. Please check your credentials and try again."
                };
            }

            if (!user.IsActive)
            {
                return new AuthResponse 
                { 
                    Succeeded = false,
                    Message = "Your account has been deactivated. Please contact support."
                };
            }

            var authResult = await GenerateAuthenticationAsync(user);
            return authResult;
        }
        catch (Exception ex)
        {
            return new AuthResponse
            {
                Succeeded = false,
                Message = $"Sign in failed: {ex.Message}"
            };
        }
    }

    private async Task<AuthResponse> GenerateAuthenticationAsync(User user)
    {
        var token = _tokenService.GenerateJwtToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        user.LastLoginAt = DateTime.UtcNow;
        
        await _unitOfWork.Users.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return new AuthResponse
        {
            Succeeded = true,
            Token = token,
            RefreshToken = refreshToken,
            TokenExpiry = DateTime.UtcNow.AddHours(1),
            User = _mapper.Map<UserDto>(user),
            Message = "Sign in successful!"
        };
    }
}
