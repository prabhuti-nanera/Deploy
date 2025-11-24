namespace CRC.WebPortal.Domain.Interfaces;

public interface IEmailService
{
    Task SendPasswordResetEmailAsync(string email, string resetLinkOrToken);
    Task SendEmailAsync(string email, string subject, string body);
}
