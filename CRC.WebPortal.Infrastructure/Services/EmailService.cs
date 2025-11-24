using CRC.WebPortal.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;

namespace CRC.WebPortal.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly bool _isDevelopment;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
        _isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
    }

    public async Task SendPasswordResetEmailAsync(string email, string resetLinkOrToken)
    {
        if (_isDevelopment)
        {
            // Development: log token and example link to console
            var sampleLink = $"/reset-password?email={Uri.EscapeDataString(email)}&token={Uri.EscapeDataString(resetLinkOrToken)}";
            Console.WriteLine($"[PasswordReset] Email: {email}\nToken: {resetLinkOrToken}\nLink: {sampleLink}");
            return;
        }

        // Production: Send actual email
        await SendActualEmailAsync(email, "Password Reset - CRC WebPortal", 
            $"Your password reset token is: {resetLinkOrToken}");
    }

    public async Task SendEmailAsync(string email, string subject, string body)
    {
        if (_isDevelopment)
        {
            // Development: log email details to console
            Console.WriteLine($"[Email] To: {email}");
            Console.WriteLine($"[Email] Subject: {subject}");
            Console.WriteLine($"[Email] Body: {body}");
            
            // Extract OTP from body for easy viewing
            if (body.Contains("<strong>") && body.Contains("</strong>"))
            {
                var start = body.IndexOf("<strong>") + 8;
                var end = body.IndexOf("</strong>");
                var otp = body.Substring(start, end - start);
                Console.WriteLine($"🔑 **YOUR OTP CODE: {otp}** 🔑");
            }
            return;
        }

        // Production: Send actual email
        await SendActualEmailAsync(email, subject, body);
    }

    private async Task SendActualEmailAsync(string toEmail, string subject, string body)
    {
        try
        {
            // Configure these in appsettings.json for production
            var smtpHost = _configuration["EmailSettings:SmtpHost"] ?? "smtp.gmail.com";
            var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"] ?? "587");
            var fromEmail = _configuration["EmailSettings:FromEmail"] ?? "your-email@gmail.com";
            var fromPassword = _configuration["EmailSettings:FromPassword"] ?? "your-app-password";

            using var client = new SmtpClient(smtpHost, smtpPort)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(fromEmail, fromPassword)
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(fromEmail, "CRC WebPortal"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            
            mailMessage.To.Add(toEmail);
            await client.SendMailAsync(mailMessage);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to send email: {ex.Message}");
            // In production, you might want to log this properly or use a fallback
        }
    }
}
