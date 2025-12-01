using ApplicationHub.Modules.Dtos.Requests;
using ApplicationHub.Modules.Entities;
using ApplicationHub.Modules.Enums;
using FluentValidation;

namespace ApplicationHub.Modules.Validators;

public class PaginatedApplicationFormRequestValidator : AbstractValidator<PaginatedRequest<ApplicationForm, PagedApplicationFormRequest>>
{
    public PaginatedApplicationFormRequestValidator()
    {
        RuleFor(x => x.Model!.UserId)
            .NotEmpty()
            .WithMessage("UserId is required.");

        RuleFor(x => x.PageNumber)
            .NotEmpty()
            .WithMessage("PageNumber is required.");
        
        RuleFor(x => x.PageSize)
            .NotEmpty()
            .WithMessage("PageSize is required.");
    }
}