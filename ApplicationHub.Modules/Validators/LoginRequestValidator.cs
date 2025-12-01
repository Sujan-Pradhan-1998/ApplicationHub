using ApplicationHub.Modules.Dtos.Requests;
using FluentValidation;

namespace ApplicationHub.Modules.Validators;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email is required.");
        
        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("Email is not a valid.");
        
        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password is required.");
    }
}
