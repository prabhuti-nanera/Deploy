using FluentValidation;

namespace CRC.WebPortal.Application.Features.Units.Commands.CreateUnit;

public class CreateUnitCommandValidator : AbstractValidator<CreateUnitCommand>
{
    public CreateUnitCommandValidator()
    {
        RuleFor(x => x.Request.UnitNumber)
            .NotEmpty().WithMessage("Unit number is required.")
            .MaximumLength(20).WithMessage("Unit number cannot exceed 20 characters.");

        RuleFor(x => x.Request.Floor)
            .GreaterThan(0).WithMessage("Floor must be greater than 0.")
            .LessThanOrEqualTo(100).WithMessage("Floor cannot exceed 100.");

        RuleFor(x => x.Request.Position)
            .GreaterThan(0).WithMessage("Position must be greater than 0.")
            .LessThanOrEqualTo(50).WithMessage("Position cannot exceed 50.");

        RuleFor(x => x.Request.BuildingId)
            .GreaterThan(0).WithMessage("Building ID is required.");
    }
}
