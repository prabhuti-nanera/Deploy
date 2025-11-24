using FluentValidation;

namespace CRC.WebPortal.Application.Features.Units.Commands.GenerateUnits;

public class GenerateUnitsCommandValidator : AbstractValidator<GenerateUnitsCommand>
{
    public GenerateUnitsCommandValidator()
    {
        RuleFor(x => x.Request.ProjectId)
            .GreaterThan(0).WithMessage("Project ID is required.");

        RuleFor(x => x.Request.UnitsData)
            .NotEmpty().WithMessage("Units data is required.");

        RuleForEach(x => x.Request.UnitsData).ChildRules(unit =>
        {
            unit.RuleFor(u => u.UnitNumber)
                .NotEmpty().WithMessage("Unit number is required.")
                .MaximumLength(20).WithMessage("Unit number cannot exceed 20 characters.");

            unit.RuleFor(u => u.FloorNumber)
                .GreaterThanOrEqualTo(0).WithMessage("Floor number must be greater than or equal to 0.");

            unit.RuleFor(u => u.BuildingName)
                .NotEmpty().WithMessage("Building name is required.");

            unit.RuleFor(u => u.Position)
                .GreaterThan(0).WithMessage("Position must be greater than 0.");
        });
    }
}
