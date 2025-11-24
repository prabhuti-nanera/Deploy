using AutoMapper;
using CRC.WebPortal.Application.Common.Models;
using CRC.WebPortal.Application.Common;
using CRC.WebPortal.Domain.Interfaces;
using MediatR;

namespace CRC.WebPortal.Application.Features.Auth.Commands.VerifyOtp;

public class VerifyOtpCommandHandler : IRequestHandler<VerifyOtpCommand, AuthResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public VerifyOtpCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<AuthResponse> Handle(VerifyOtpCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Validate input format
            if (string.IsNullOrWhiteSpace(request.Email) || !IsValidEmail(request.Email))
            {
                return new AuthResponse
                {
                    Succeeded = false,
                    Message = "Please enter a valid email address."
                };
            }

            if (string.IsNullOrWhiteSpace(request.OtpCode) || !IsValidOtp(request.OtpCode))
            {
                return new AuthResponse
                {
                    Succeeded = false,
                    Message = "Please enter a valid 4-digit OTP."
                };
            }

            if (string.IsNullOrWhiteSpace(request.NewPassword) || request.NewPassword.Length < 6)
            {
                return new AuthResponse
                {
                    Succeeded = false,
                    Message = "Password must be at least 6 characters long."
                };
            }

            // Find user by email
            var user = await _unitOfWork.Users.GetByEmailAsync(request.Email.ToLowerInvariant());
            
            if (user == null)
            {
                return new AuthResponse
                {
                    Succeeded = false,
                    Message = "Email not found."
                };
            }

            // Check if user account is active
            if (!user.IsActive)
            {
                return new AuthResponse
                {
                    Succeeded = false,
                    Message = "Account is inactive. Please contact support."
                };
            }

            // Check if OTP exists
            if (string.IsNullOrEmpty(user.OtpCode) || user.OtpExpiry == null)
            {
                return new AuthResponse
                {
                    Succeeded = false,
                    Message = "No OTP found. Please request a new OTP."
                };
            }

            // Check if OTP is expired
            if (user.OtpExpiry < DateTime.UtcNow)
            {
                return new AuthResponse
                {
                    Succeeded = false,
                    Message = "OTP has expired. Please request a new OTP."
                };
            }

            // Check if OTP matches
            if (user.OtpCode != request.OtpCode)
            {
                return new AuthResponse
                {
                    Succeeded = false,
                    Message = "Invalid OTP. Please check and try again."
                };
            }

            // Update password and clear OTP
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            user.OtpCode = null;
            user.OtpExpiry = null;
            user.UpdatedAt = DateTime.UtcNow;
            
            await _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            return new AuthResponse
            {
                Succeeded = true,
                Message = "Password reset successfully."
            };
        }
        catch (Exception ex)
        {
            return new AuthResponse
            {
                Succeeded = false,
                Message = $"Failed to reset password: {ex.Message}"
            };
        }
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    private static bool IsValidOtp(string otp)
    {
        return otp.Length == 4 && otp.All(char.IsDigit);
    }
}
