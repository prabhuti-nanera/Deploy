using FluentValidation;

namespace CRC.WebPortal.Application.Features.Projects.Commands.CreateProject;

public class CreateProjectCommandValidator : AbstractValidator<CreateProjectCommand>
{
    public CreateProjectCommandValidator()
    {
        RuleFor(x => x.Request.Name)
            .NotEmpty().WithMessage("Project name is required.")
            .MaximumLength(200).WithMessage("Project name cannot exceed 200 characters.");

        RuleFor(x => x.Request.Address)
            .NotEmpty().WithMessage("Project address is required.")
            .MaximumLength(500).WithMessage("Project address cannot exceed 500 characters.");

        RuleFor(x => x.Request.Type)
            .NotEmpty().WithMessage("Project type is required.")
            .Must(type => new[] { "Res", "Com", "Other" }.Contains(type))
            .WithMessage("Project type must be 'Res', 'Com', or 'Other'.");

        RuleFor(x => x.Request.AdminName)
            .NotEmpty().WithMessage("Admin name is required.")
            .MaximumLength(100).WithMessage("Admin name cannot exceed 100 characters.");

        RuleFor(x => x.Request.AdminMobile)
            .NotEmpty().WithMessage("Admin mobile is required.")
            .Matches(@"^\+?[1-9]\d{1,14}$").WithMessage("Please enter a valid mobile number.");

    }
}
