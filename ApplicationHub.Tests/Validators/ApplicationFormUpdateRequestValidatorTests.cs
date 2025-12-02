using ApplicationHub.Modules.Dtos.Requests;
using ApplicationHub.Modules.Enums;
using ApplicationHub.Modules.Validators;
using FluentValidation.TestHelper;

namespace ApplicationHub.Tests.Validators;

public class ApplicationFormUpdateRequestValidatorTests
{
    private readonly ApplicationFormUpdateRequestValidator _validator;

    public ApplicationFormUpdateRequestValidatorTests()
    {
        _validator = new ApplicationFormUpdateRequestValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        var model = new ApplicationFormUpdateRequest
        {
            Id = Guid.Empty,
            Company = "Test",
            Position = "Test",
            FormStatus = ApplicationFormStatusEnum.ApplicationReceivedStage
        };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public void Should_Have_Error_When_UserId_Is_Empty()
    {
        var model = new ApplicationFormUpdateRequest
        {
            UserId = Guid.Empty,
            Position = "Test",
            FormStatus = ApplicationFormStatusEnum.ApplicationReceivedStage,
            Company = "Test",
            Id = Guid.NewGuid(),
        };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }

    [Fact]
    public void Should_Have_Error_When_AppliedOn_Is_Default()
    {
        var model = new ApplicationFormUpdateRequest
        {
            AppliedOn = default,
            Id = Guid.NewGuid(),
            Position = "Test",
            FormStatus = ApplicationFormStatusEnum.ApplicationReceivedStage,
            Company = "Test",
        };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.AppliedOn);
    }

    [Fact]
    public void Should_Have_Error_When_Company_Is_Empty()
    {
        var model = new ApplicationFormUpdateRequest
        {
            Company = "",
            Id = Guid.NewGuid(),
            Position = "Test",
            FormStatus = ApplicationFormStatusEnum.ApplicationReceivedStage
        };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Company);
    }

    [Fact]
    public void Should_Have_Error_When_Position_Is_Empty()
    {
        var model = new ApplicationFormUpdateRequest
        {
            Position = "",
            Id = Guid.NewGuid(),
            Company = "Test",
            FormStatus = ApplicationFormStatusEnum.ApplicationReceivedStage
        };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Position);
    }

    [Fact]
    public void Should_Have_Error_When_Description_Is_Empty()
    {
        var model = new ApplicationFormUpdateRequest
        {
            Description = "",
            Id = Guid.NewGuid(),
            Position = "Test",
            FormStatus = ApplicationFormStatusEnum.ApplicationReceivedStage,
            Company = "Test",
        };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void Should_Have_Error_When_FormStatus_Is_Invalid()
    {
        var invalidStatus = (ApplicationFormStatusEnum)(-1);
        var model = new ApplicationFormUpdateRequest
        {
            Id = Guid.NewGuid(),
            Position = "Test",
            Company = "Test",
            FormStatus = invalidStatus
        };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.FormStatus);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Model_Is_Valid()
    {
        var model = new ApplicationFormUpdateRequest
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            AppliedOn = DateTime.UtcNow,
            Company = "Test Company",
            Position = "Developer",
            Description = "Job description",
            FormStatus = ApplicationFormStatusEnum.ApplicationReceivedStage
        };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }
}