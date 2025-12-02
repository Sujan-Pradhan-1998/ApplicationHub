using ApplicationHub.Modules.Dtos.Requests;
using ApplicationHub.Modules.Enums;
using ApplicationHub.Modules.Validators;
using FluentValidation.TestHelper;

namespace ApplicationHub.Tests.Validators;

public class ApplicationFormRequestValidatorTests
{
    private readonly ApplicationFormRequestValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_UserId_Is_Empty()
    {
        var model = new ApplicationFormRequest
        {
            UserId = Guid.Empty,
            Company = "Test",
            Position = "Test",
            FormStatus = ApplicationFormStatusEnum.ApplicationReceivedStage
        };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }

    [Fact]
    public void Should_Have_Error_When_AppliedOn_Is_Default()
    {
        var model = new ApplicationFormRequest
        {
            UserId = Guid.NewGuid(),
            Company = "Test",
            Position = "Test",
            FormStatus = ApplicationFormStatusEnum.ApplicationReceivedStage
        };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.AppliedOn);
    }

    [Fact]
    public void Should_Have_Error_When_Company_Is_Empty()
    {
        var model = new ApplicationFormRequest
        {
            UserId = Guid.NewGuid(),
            Company = "",
            Position = "Test",
            FormStatus = ApplicationFormStatusEnum.ApplicationReceivedStage
        };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Company);
    }

    [Fact]
    public void Should_Have_Error_When_Position_Is_Empty()
    {
        var model = new ApplicationFormRequest
        {
            UserId = Guid.NewGuid(),
            Company = "Test",
            Position = "",
            FormStatus = ApplicationFormStatusEnum.ApplicationReceivedStage
        };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Position);
    }

    [Fact]
    public void Should_Have_Error_When_Description_Is_Empty()
    {
        var model = new ApplicationFormRequest
        {
            UserId = Guid.NewGuid(),
            Company = "Test",
            Position = "Test",
            Description = "",
            FormStatus = ApplicationFormStatusEnum.ApplicationReceivedStage
        };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void Should_Have_Error_When_FormStatus_Is_Invalid()
    {
        var model = new ApplicationFormRequest
        {
            UserId = Guid.NewGuid(),
            Company = "Test",
            Position = "Test",
            FormStatus = (ApplicationFormStatusEnum)999
        };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.FormStatus);
    }

    [Fact]
    public void Should_Not_Have_Error_When_All_Properties_Are_Valid()
    {
        var model = new ApplicationFormRequest
        {
            UserId = Guid.NewGuid(),
            AppliedOn = DateTime.Now.AddDays(-1),
            Company = "CompanyX",
            Position = "Developer",
            Description = "Application description",
            FormStatus = ApplicationFormStatusEnum.ApplicationReceivedStage
        };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }
}