using AutoMapper;
using CRC.WebPortal.Application.Common.Models;
using CRC.WebPortal.Application.Common;
using CRC.WebPortal.Domain.Entities;
using CRC.WebPortal.Domain.Interfaces;
using MediatR;

namespace CRC.WebPortal.Application.Features.Auth.Commands.SignUp;

public class SignUpCommandHandler : IRequestHandler<SignUpCommand, AuthResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;

    public SignUpCommandHandler(IUnitOfWork unitOfWork, ITokenService tokenService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
        _mapper = mapper;
    }

    public async Task<AuthResponse> Handle(SignUpCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (await _unitOfWork.Users.EmailExistsAsync(request.Email))
            {
                return new AuthResponse 
                { 
                    Succeeded = false,
                    Message = "An account with this email address already exists. Please use a different email or sign in."
                };
            }

            var user = await CreateUserAsync(request);
            var authResult = await GenerateAuthenticationAsync(user);

            return authResult;
        }
        catch (Exception ex)
        {
            return new AuthResponse
            {
                Succeeded = false,
                Message = $"Registration failed: {ex.Message}"
            };
        }
    }

    private async Task<User> CreateUserAsync(SignUpCommand request)
    {
        var user = new User
        {
            Email = request.Email.ToLowerInvariant(),
            FirstName = request.FirstName,
            LastName = request.LastName,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            IsActive = true,
            IsEmailVerified = false
        };

        await _unitOfWork.Users.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return user;
    }

    private async Task<AuthResponse> GenerateAuthenticationAsync(User user)
    {
        var token = _tokenService.GenerateJwtToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
        await _unitOfWork.Users.UpdateAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return new AuthResponse
        {
            Succeeded = true,
            Token = token,
            RefreshToken = refreshToken,
            TokenExpiry = DateTime.UtcNow.AddHours(1),
            User = _mapper.Map<UserDto>(user)
        };
    }
}
