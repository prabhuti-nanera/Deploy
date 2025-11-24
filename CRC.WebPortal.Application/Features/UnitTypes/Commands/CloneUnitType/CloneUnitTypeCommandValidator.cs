using FluentValidation;

namespace CRC.WebPortal.Application.Features.UnitTypes.Commands.CloneUnitType;

public class CloneUnitTypeCommandValidator : AbstractValidator<CloneUnitTypeCommand>
{
    public CloneUnitTypeCommandValidator()
    {
        RuleFor(x => x.Request.SourceUnitTypeId)
            .GreaterThan(0)
            .WithMessage("Source unit type ID must be greater than 0.");

        RuleFor(x => x.Request.NewName)
            .NotEmpty()
            .WithMessage("New unit type name is required.")
            .MaximumLength(50)
            .WithMessage("Unit type name cannot exceed 50 characters.");

        RuleFor(x => x.Request.NewDescription)
            .MaximumLength(200)
            .WithMessage("Unit type description cannot exceed 200 characters.")
            .When(x => !string.IsNullOrEmpty(x.Request.NewDescription));
    }
}
