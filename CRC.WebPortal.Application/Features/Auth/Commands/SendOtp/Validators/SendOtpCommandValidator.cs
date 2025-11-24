using FluentValidation;

namespace CRC.WebPortal.Application.Features.Auth.Commands.SendOtp.Validators;

public class SendOtpCommandValidator : AbstractValidator<SendOtpCommand>
{
    public SendOtpCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Please enter a valid email address.")
            .MaximumLength(254)
            .WithMessage("Email address is too long.");
    }
}
