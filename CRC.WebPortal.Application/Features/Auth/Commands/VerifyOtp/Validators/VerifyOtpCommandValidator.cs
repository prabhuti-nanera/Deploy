using FluentValidation;

namespace CRC.WebPortal.Application.Features.Auth.Commands.VerifyOtp.Validators;

public class VerifyOtpCommandValidator : AbstractValidator<VerifyOtpCommand>
{
    public VerifyOtpCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required.")
            .EmailAddress()
            .WithMessage("Please enter a valid email address.")
            .MaximumLength(254)
            .WithMessage("Email address is too long.");

        RuleFor(x => x.OtpCode)
            .NotEmpty()
            .WithMessage("OTP is required.")
            .Length(4)
            .WithMessage("OTP must be exactly 4 digits.")
            .Matches(@"^\d{4}$")
            .WithMessage("OTP must contain only numbers.");

        RuleFor(x => x.NewPassword)
            .NotEmpty()
            .WithMessage("New Password is required.")
            .MinimumLength(6)
            .WithMessage("Password must be at least 6 characters long.")
            .MaximumLength(100)
            .WithMessage("Password is too long.");
    }
}
