using ApplicationHub.Modules.Dtos.Requests;
using ApplicationHub.Modules.Validators;
using FluentValidation.TestHelper;

namespace ApplicationHub.Tests.Validators;

public class LoginRequestValidatorTests
{
    private readonly LoginRequestValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_Email_Is_Empty()
    {
        var model = new LoginRequest { Email = "", Password = "Password123!" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage("Email is required.");
    }

    [Fact]
    public void Should_Have_Error_When_Email_Is_Invalid()
    {
        var model = new LoginRequest { Email = "invalid-email", Password = "Password123!" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Email)
            .WithErrorMessage("Email is not a valid.");
    }

    [Fact]
    public void Should_Have_Error_When_Password_Is_Empty()
    {
        var model = new LoginRequest { Email = "test@example.com", Password = "" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Password)
            .WithErrorMessage("Password is required.");
    }

    [Fact]
    public void Should_Not_Have_Error_When_Model_Is_Valid()
    {
        var model = new LoginRequest { Email = "test@example.com", Password = "Password123!" };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }
}