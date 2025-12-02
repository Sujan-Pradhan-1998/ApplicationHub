using ApplicationHub.Api;
using ApplicationHub.Api.Controllers;
using ApplicationHub.Data.Services.IServices;
using ApplicationHub.Modules.Dtos.Requests;
using ApplicationHub.Modules.Dtos.Responses;
using ApplicationHub.Modules.Entities;
using ApplicationHub.Modules.Enums;
using ApplicationHub.Modules.Models;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ApplicationHub.Tests.Controllers;

public class ApplicationFormControllerTests
{
    private readonly Mock<IApplicationFormService> _service;
    private readonly Mock<IValidator<ApplicationFormRequest>> _createValidator;
    private readonly Mock<IValidator<ApplicationFormUpdateRequest>> _updateValidator;
    private readonly Mock<IValidator<PaginatedRequest<ApplicationForm, PagedApplicationFormRequest>>> _pagedValidator;

    private readonly ApplicationFormController _controller;

    public ApplicationFormControllerTests()
    {
        _service = new Mock<IApplicationFormService>();
        Mock<IUserMeta> userMeta = new Mock<IUserMeta>();
        _createValidator = new Mock<IValidator<ApplicationFormRequest>>();
        _updateValidator = new Mock<IValidator<ApplicationFormUpdateRequest>>();
        _pagedValidator = new Mock<IValidator<PaginatedRequest<ApplicationForm, PagedApplicationFormRequest>>>();

        userMeta.Setup(x => x.Id).Returns(Guid.Parse("87aaf02b-2104-4e15-926d-62e30258ae37"));

        _controller = new ApplicationFormController(
            _service.Object,
            userMeta.Object,
            _createValidator.Object,
            _updateValidator.Object,
            _pagedValidator.Object
        );
    }

    [Fact]
    public void GetFormStatus_ReturnsEnumNames()
    {
        var result = _controller.GetFormStatus() as OkObjectResult;

        Assert.NotNull(result);
        var statuses = result.Value as string[];

        Assert.NotNull(statuses);
        Assert.Contains(nameof(ApplicationFormStatusEnum.AssessmentStage), statuses);
    }

    [Fact]
    public async Task GetApplicationFormById_NotFound_ReturnsNotFound()
    {
        _service.Setup(s => s.GetApplicationFormById(It.IsAny<Guid>()))
            .ReturnsAsync((ApplicationFormResponse?)null);

        var result = await _controller.GetApplicationFormById(Guid.Parse("87aaf02b-2104-4e15-926d-62e30258ae37"));

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task GetApplicationFormById_Found_ReturnsOk()
    {
        _service.Setup(s => s.GetApplicationFormById(It.IsAny<Guid>()))
            .ReturnsAsync(new ApplicationFormResponse()
            {
                Company = "Test Company",
                Id = Guid.Parse("87aaf02b-2104-4e15-926d-62e30258ae37"),
                Position = "Test Position",
                Description = "Test Description",
                FormStatus = ApplicationFormStatusEnum.AssessmentStage,
                UserId = Guid.Parse("87aaf02b-2104-4e15-926d-62e30258ae37"),
                AppliedOn = DateTime.Now,
                CreatedOn = DateTime.Now
            });

        var result = await _controller.GetApplicationFormById(Guid.Parse("87aaf02b-2104-4e15-926d-62e30258ae37"));

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task GetMyApplicationForms_InvalidRequest_ReturnsBadRequest()
    {
        _pagedValidator.Setup(v => v.ValidateAsync(
                It.IsAny<PaginatedRequest<ApplicationForm, PagedApplicationFormRequest>>(),
                CancellationToken.None))
            .ReturnsAsync(new ValidationResult(new[] { new ValidationFailure("Field", "Error") }));

        var result = await _controller.GetMyApplicationForms(new());

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task GetMyApplicationForms_ValidRequest_ReturnsOk()
    {
        _pagedValidator.Setup(v => v.ValidateAsync(
                It.IsAny<PaginatedRequest<ApplicationForm, PagedApplicationFormRequest>>(),
                CancellationToken.None))
            .ReturnsAsync(new ValidationResult());

        _service.Setup(s =>
                s.GetMyApplicationForms(It.IsAny<PaginatedRequest<ApplicationForm, PagedApplicationFormRequest>>()))
            .ReturnsAsync(new PaginatedResponse<ApplicationFormResponse>());

        var result = await _controller.GetMyApplicationForms(new());

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Post_InvalidModel_ReturnsBadRequest()
    {
        _createValidator.Setup(v => v.ValidateAsync(It.IsAny<ApplicationFormRequest>(), default))
            .ReturnsAsync(new ValidationResult(new[]
            {
                new ValidationFailure("Name", "Required")
            }));

        var result = await _controller.Post(new ApplicationFormRequest()
        {
            Company = "Test Company",
            Position = "Test Position",
            Description = "Test Description",
            FormStatus = ApplicationFormStatusEnum.AssessmentStage,
            UserId = Guid.Parse("87aaf02b-2104-4e15-926d-62e30258ae37"),
            AppliedOn = DateTime.Now
        });

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Post_ServiceReturnsNull_ReturnsBadRequest()
    {
        _createValidator.Setup(v => v.ValidateAsync(It.IsAny<ApplicationFormRequest>(), default))
            .ReturnsAsync(new ValidationResult());

        _service.Setup(s => s.AddApplicationForm(It.IsAny<ApplicationFormRequest>()))
            .ReturnsAsync((ApplicationFormResponse?)null);

        var result = await _controller.Post(new ApplicationFormRequest()
        {
            Company = "Test Company",
            Position = "Test Position",
            Description = "Test Description",
            FormStatus = ApplicationFormStatusEnum.AssessmentStage,
            UserId = Guid.Parse("87aaf02b-2104-4e15-926d-62e30258ae37"),
            AppliedOn = DateTime.Now
        });

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Post_ValidModel_ReturnsOk()
    {
        _createValidator.Setup(v => v.ValidateAsync(It.IsAny<ApplicationFormRequest>(), default))
            .ReturnsAsync(new ValidationResult());

        _service.Setup(s => s.AddApplicationForm(It.IsAny<ApplicationFormRequest>()))
            .ReturnsAsync(new ApplicationFormResponse()
            {
                Company = "Test Company",
                Id = Guid.Parse("87aaf02b-2104-4e15-926d-62e30258ae37"),
                Position = "Test Position",
                Description = "Test Description",
                FormStatus = ApplicationFormStatusEnum.AssessmentStage,
                UserId = Guid.Parse("87aaf02b-2104-4e15-926d-62e30258ae37"),
                AppliedOn = DateTime.Now,
                CreatedOn = DateTime.Now
            });

        var result = await _controller.Post(new ApplicationFormRequest()
        {
            Company = "Test Company",
            Position = "Test Position",
            Description = "Test Description",
            FormStatus = ApplicationFormStatusEnum.AssessmentStage,
            UserId = Guid.Parse("87aaf02b-2104-4e15-926d-62e30258ae37"),
            AppliedOn = DateTime.Now
        });

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task Put_InvalidModel_ReturnsBadRequest()
    {
        _updateValidator.Setup(v => v.ValidateAsync(It.IsAny<ApplicationFormUpdateRequest>(), default))
            .ReturnsAsync(new ValidationResult(new[]
            {
                new ValidationFailure("Name", "Invalid")
            }));

        var result = await _controller.Put(new ApplicationFormUpdateRequest()
        {
            Company = "Test Company",
            Id = Guid.Parse("87aaf02b-2104-4e15-926d-62e30258ae37"),
            Position = "Test Position",
            Description = "Test Description",
            FormStatus = ApplicationFormStatusEnum.AssessmentStage,
            UserId = Guid.Parse("87aaf02b-2104-4e15-926d-62e30258ae37"),
            AppliedOn = DateTime.Now
        });

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Put_ServiceReturnsNull_ReturnsBadRequest()
    {
        _updateValidator.Setup(v => v.ValidateAsync(It.IsAny<ApplicationFormUpdateRequest>(), default))
            .ReturnsAsync(new ValidationResult());

        _service.Setup(s => s.UpdateApplicationForm(It.IsAny<ApplicationFormUpdateRequest>()))
            .ReturnsAsync((ApplicationFormResponse?)null);

        var result = await _controller.Put(new ApplicationFormUpdateRequest()
        {
            Company = "Test Company",
            Id = Guid.Parse("87aaf02b-2104-4e15-926d-62e30258ae37"),
            Position = "Test Position",
            Description = "Test Description",
            FormStatus = ApplicationFormStatusEnum.AssessmentStage,
            UserId = Guid.Parse("87aaf02b-2104-4e15-926d-62e30258ae37"),
            AppliedOn = DateTime.Now
        });

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Put_ValidModel_ReturnsOk()
    {
        _updateValidator.Setup(v => v.ValidateAsync(It.IsAny<ApplicationFormUpdateRequest>(), default))
            .ReturnsAsync(new ValidationResult());

        _service.Setup(s => s.UpdateApplicationForm(It.IsAny<ApplicationFormUpdateRequest>()))
            .ReturnsAsync(new ApplicationFormResponse()
            {
                Company = "Test Company",
                Id = Guid.Parse("87aaf02b-2104-4e15-926d-62e30258ae37"),
                Position = "Test Position",
                Description = "Test Description",
                FormStatus = ApplicationFormStatusEnum.AssessmentStage,
                UserId = Guid.Parse("87aaf02b-2104-4e15-926d-62e30258ae37"),
                AppliedOn = DateTime.Now,
                CreatedOn = DateTime.Now
            });

        var result = await _controller.Put(new ApplicationFormUpdateRequest()
        {
            Company = "Test Company",
            Id = Guid.Parse("87aaf02b-2104-4e15-926d-62e30258ae37"),
            Position = "Test Position",
            Description = "Test Description",
            FormStatus = ApplicationFormStatusEnum.AssessmentStage,
            UserId = Guid.Parse("87aaf02b-2104-4e15-926d-62e30258ae37"),
            AppliedOn = DateTime.Now
        });

        Assert.IsType<OkObjectResult>(result);
    }

    [Fact]
    public async Task DeleteApplicationForm_Failure_ReturnsBadRequest()
    {
        _service.Setup(s => s.DeleteApplicationFormById(It.IsAny<Guid>()))
            .ReturnsAsync(false);

        var result = await _controller.DeleteApplicationForm(Guid.Parse("87aaf02b-2104-4e15-926d-62e30258ae37"));

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task DeleteApplicationForm_Success_ReturnsOk()
    {
        _service.Setup(s => s.DeleteApplicationFormById(It.IsAny<Guid>()))
            .ReturnsAsync(true);

        var result = await _controller.DeleteApplicationForm(Guid.Parse("87aaf02b-2104-4e15-926d-62e30258ae37"));

        Assert.IsType<OkObjectResult>(result);
    }
}