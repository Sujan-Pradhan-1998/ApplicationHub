using ApplicationHub.Modules.Dtos.Requests;
using ApplicationHub.Modules.Enums;
using FluentValidation;

namespace ApplicationHub.Modules.Validators;

public class ApplicationFormUpdateRequestValidator : AbstractValidator<ApplicationFormUpdateRequest>
{
    public ApplicationFormUpdateRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required.");
        
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("UserId is required.");

        RuleFor(x => x.AppliedOn)
            .NotEmpty()
            .WithMessage("AppliedOn is required.");

        RuleFor(x => x.Company)
            .NotEmpty()
            .WithMessage("Company is required.");

        RuleFor(x => x.Position)
            .NotEmpty()
            .WithMessage("Position is required.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required.");

        RuleFor(x => x.FormStatus)
            .Must(x =>
                x is >= ApplicationFormStatusEnum.ApplicationReceivedStage
                    and <= ApplicationFormStatusEnum.PositionFilledStage
            )
            .WithMessage("FormStatus is required.");
    }
}