using FluentValidation;

namespace CRC.WebPortal.Application.Features.UnitTypes.Commands.UpdateUnitType;

public class UpdateUnitTypeCommandValidator : AbstractValidator<UpdateUnitTypeCommand>
{
    public UpdateUnitTypeCommandValidator()
    {
        RuleFor(x => x.Request.Id)
            .GreaterThan(0).WithMessage("Valid unit type ID is required.");

        RuleFor(x => x.Request.Name)
            .NotEmpty().WithMessage("Unit type name is required.")
            .MaximumLength(50).WithMessage("Unit type name cannot exceed 50 characters.")
            .Matches(@"^[a-zA-Z0-9\s\-_]+$").WithMessage("Unit type name can only contain letters, numbers, spaces, hyphens, and underscores.");

        RuleFor(x => x.Request.Description)
            .MaximumLength(200).WithMessage("Description cannot exceed 200 characters.")
            .When(x => !string.IsNullOrEmpty(x.Request.Description));

        RuleFor(x => x.Request.DisplayOrder)
            .GreaterThanOrEqualTo(0).WithMessage("Display order must be a non-negative number.");
    }
}
