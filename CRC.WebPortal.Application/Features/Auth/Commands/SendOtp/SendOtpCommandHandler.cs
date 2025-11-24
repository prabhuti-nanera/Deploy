using AutoMapper;
using CRC.WebPortal.Application.Common.Models;
using CRC.WebPortal.Application.Common;
using CRC.WebPortal.Domain.Interfaces;
using MediatR;

namespace CRC.WebPortal.Application.Features.Auth.Commands.SendOtp;

public class SendOtpCommandHandler : IRequestHandler<SendOtpCommand, AuthResponse>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IEmailService _emailService;
    private readonly IMapper _mapper;

    public SendOtpCommandHandler(IUnitOfWork unitOfWork, IEmailService emailService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _emailService = emailService;
        _mapper = mapper;
    }

    public async Task<AuthResponse> Handle(SendOtpCommand request, CancellationToken cancellationToken)
    {
        try
        {
            // Validate email format
            if (string.IsNullOrWhiteSpace(request.Email) || !IsValidEmail(request.Email))
            {
                return new AuthResponse
                {
                    Succeeded = false,
                    Message = "Please enter a valid email address."
                };
            }

            // Check if user exists
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

            // Rate limiting: Check if OTP was recently sent (prevent spam)
            if (user.OtpExpiry.HasValue && user.OtpExpiry > DateTime.UtcNow.AddMinutes(-1))
            {
                return new AuthResponse
                {
                    Succeeded = false,
                    Message = "Please wait before requesting another OTP."
                };
            }

            // Generate 4-digit OTP
            var random = new Random();
            var otpCode = random.Next(1000, 9999).ToString();
            
            // Set OTP and expiry (5 minutes from now)
            user.OtpCode = otpCode;
            user.OtpExpiry = DateTime.UtcNow.AddMinutes(5);
            
            await _unitOfWork.Users.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();

            // Send OTP via email
            var emailSubject = "Password Reset OTP - CRC WebPortal";
            var emailBody = $@"
                <h2>Password Reset OTP</h2>
                <p>Your OTP for password reset is: <strong>{otpCode}</strong></p>
                <p>This OTP will expire in 5 minutes.</p>
                <p>If you didn't request this, please ignore this email.</p>
            ";

            await _emailService.SendEmailAsync(user.Email, emailSubject, emailBody);

            return new AuthResponse
            {
                Succeeded = true,
                Message = "OTP sent successfully to your email.",
                // Include OTP in a separate property for development mode console logging
                RefreshToken = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development" ? otpCode : null
            };
        }
        catch (Exception ex)
        {
            return new AuthResponse
            {
                Succeeded = false,
                Message = $"Failed to send OTP: {ex.Message}"
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
}
