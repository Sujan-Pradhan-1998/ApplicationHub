using ApplicationHub.Data.Repos.IRepos;
using ApplicationHub.Data.Services;
using ApplicationHub.Modules.Dtos.Requests;
using ApplicationHub.Modules.Entities;
using ApplicationHub.Modules.Enums;
using ApplicationHub.Modules.Models;
using Moq;

namespace ApplicationHub.Tests.Data.Services;

public class ApplicationFormServiceTests
{
    private readonly Mock<IApplicationFormRepository> _repositoryMock;
    private readonly ApplicationFormService _service;

    public ApplicationFormServiceTests()
    {
        _repositoryMock = new Mock<IApplicationFormRepository>();
        _service = new ApplicationFormService(_repositoryMock.Object);
    }

    [Fact]
    public async Task GetApplicationFormById_ReturnsMappedResponse()
    {
        var formId = Guid.NewGuid();
        var entity = new ApplicationForm
        {
            Id = formId,
            UserId = Guid.NewGuid(),
            Company = "Test",
            Position = "Test",
            FormStatus = ApplicationFormStatusEnum.ApplicationReceivedStage
        };
        _repositoryMock.Setup(r => r.GetApplicationFormById(formId)).ReturnsAsync(entity);

        var result = await _service.GetApplicationFormById(formId);

        Assert.NotNull(result);
        Assert.Equal(formId, result.Id);
    }

    [Fact]
    public async Task AddApplicationForm_ReturnsMappedResponse()
    {
        var request = new ApplicationFormRequest()
        {
            UserId = Guid.NewGuid(),
            Company = "Test",
            Position = "Test",
            FormStatus = ApplicationFormStatusEnum.ApplicationReceivedStage
        };
        var entity = new ApplicationForm
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            Company = "Test",
            Position = "Test",
            FormStatus = ApplicationFormStatusEnum.ApplicationReceivedStage
        };
        _repositoryMock.Setup(r => r.AddApplicationForm(request)).ReturnsAsync(entity);

        var result = await _service.AddApplicationForm(request);

        Assert.NotNull(result);
        Assert.Equal(entity.Id, result.Id);
    }

    [Fact]
    public async Task UpdateApplicationForm_ReturnsMappedResponse()
    {
        var request = new ApplicationFormUpdateRequest
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            Company = "Test",
            Position = "Test",
            FormStatus = ApplicationFormStatusEnum.ApplicationReceivedStage
        };
        var entity = new ApplicationForm
        {
            Id = request.Id,
            UserId = Guid.NewGuid(),
            Company = "Test",
            Position = "Test",
            FormStatus = ApplicationFormStatusEnum.ApplicationReceivedStage
        };
        _repositoryMock.Setup(r => r.UpdateApplicationForm(request)).ReturnsAsync(entity);

        var result = await _service.UpdateApplicationForm(request);

        Assert.NotNull(result);
        Assert.Equal(request.Id, result.Id);
    }

    [Fact]
    public async Task DeleteApplicationFormById_ReturnsTrue()
    {
        var formId = Guid.NewGuid();
        _repositoryMock.Setup(r => r.DeleteApplicationFormById(formId)).ReturnsAsync(true);

        var result = await _service.DeleteApplicationFormById(formId);

        Assert.True(result);
    }

    [Fact]
    public async Task GetMyApplicationForms_ReturnsPaginatedResponse()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var pagedRequest = new PaginatedRequest<ApplicationForm, PagedApplicationFormRequest>
        {
            Model = new PagedApplicationFormRequest
            {
                UserId = userId
            }
        };

        var appForms = new List<ApplicationForm>
        {
            new ApplicationForm
            {
                Id = Guid.NewGuid(), UserId = userId, Company = "CompanyA", Position = "Developer",
                FormStatus = ApplicationFormStatusEnum.ApplicationReceivedStage, AppliedOn = DateTime.UtcNow
            },
            new ApplicationForm
            {
                Id = Guid.NewGuid(), UserId = userId, Company = "CompanyB", Position = "Tester",
                FormStatus = ApplicationFormStatusEnum.ApplicationReceivedStage, AppliedOn = DateTime.UtcNow
            }
        };

        var expectedResponse = new PaginatedResponse<ApplicationForm>
        {
            Items = appForms,
            TotalRecords = appForms.Count
        };

        _repositoryMock
            .Setup(r => r.GetMyApplicationForms(pagedRequest))
            .ReturnsAsync(expectedResponse);

        // Act
        var result = await _service.GetMyApplicationForms(pagedRequest);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Items.Count());
        Assert.All(result.Items, item => Assert.Equal(userId, item.UserId));
    }
}