using ApplicationHub.Modules.Dtos.Requests;
using FluentValidation;

namespace ApplicationHub.Modules.Validators;

public class UserRequestValidator : AbstractValidator<UserRequest>
{
    public UserRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required.");
    }
}
