using ApplicationHub.Data;
using ApplicationHub.Data.Repos;
using ApplicationHub.Data.Repos.IRepos;
using ApplicationHub.Modules.Dtos.Requests;
using ApplicationHub.Modules.Entities;
using ApplicationHub.Modules.Enums;
using ApplicationHub.Modules.Models;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace ApplicationHub.Tests.Data.Repos;

public class ApplicationFormRepositoryTests
{
    private AppDbContext GetDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var mockDbOption = new Mock<IDbOption>();
        return new AppDbContext(options, mockDbOption.Object);
    }

    [Fact]
    public async Task AddApplicationForm_ShouldAddAndReturnApplicationForm()
    {
        var context = GetDbContext();
        var paginationMock = new Mock<IPaginationBaseRepository<ApplicationForm, PagedApplicationFormRequest>>();
        var repo = new ApplicationFormRepository(context, paginationMock.Object);

        var request = new ApplicationFormRequest
        {
            UserId = Guid.NewGuid(),
            Company = "Test Company",
            Position = "Developer",
            Description = "Test Desc",
            AppliedOn = DateTime.UtcNow,
            FormStatus = ApplicationFormStatusEnum.AssessmentStage
        };

        var result = await repo.AddApplicationForm(request);

        Assert.NotNull(result);
        Assert.Equal(request.Company, result!.Company);
        Assert.Equal(1, await context.ApplicationForms.CountAsync());
    }

    [Fact]
    public async Task GetApplicationFormById_ShouldReturnApplicationForm_WhenExists()
    {
        var context = GetDbContext();
        var paginationMock = new Mock<IPaginationBaseRepository<ApplicationForm, PagedApplicationFormRequest>>();
        var repo = new ApplicationFormRepository(context, paginationMock.Object);

        var form = new ApplicationForm
        {
            Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Company = "A", Position = "B",
            FormStatus = ApplicationFormStatusEnum.AssessmentStage
        };
        await context.ApplicationForms.AddAsync(form);
        await context.SaveChangesAsync();

        var result = await repo.GetApplicationFormById(form.Id);

        Assert.NotNull(result);
        Assert.Equal(form.Id, result!.Id);
    }

    [Fact]
    public async Task UpdateApplicationForm_ShouldUpdateAndReturnApplicationForm()
    {
        var context = GetDbContext();
        var paginationMock = new Mock<IPaginationBaseRepository<ApplicationForm, PagedApplicationFormRequest>>();
        var repo = new ApplicationFormRepository(context, paginationMock.Object);

        var form = new ApplicationForm
        {
            Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Company = "Old", Position = "OldPos",
            FormStatus = ApplicationFormStatusEnum.AssessmentStage
        };
        await context.ApplicationForms.AddAsync(form);
        await context.SaveChangesAsync();

        var updateRequest = new ApplicationFormUpdateRequest
        {
            Id = form.Id,
            Company = "New",
            Position = "NewPos",
            Description = "Updated",
            AppliedOn = DateTime.UtcNow,
            FormStatus = ApplicationFormStatusEnum.AssessmentStage
        };

        var result = await repo.UpdateApplicationForm(updateRequest);

        Assert.NotNull(result);
        Assert.Equal("New", result!.Company);
        Assert.Equal(ApplicationFormStatusEnum.AssessmentStage, result.FormStatus);
    }

    [Fact]
    public async Task DeleteApplicationFormById_ShouldDeleteAndReturnTrue_WhenExists()
    {
        var context = GetDbContext();
        var paginationMock = new Mock<IPaginationBaseRepository<ApplicationForm, PagedApplicationFormRequest>>();
        var repo = new ApplicationFormRepository(context, paginationMock.Object);

        var form = new ApplicationForm
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            Company = "DeleteMe",
            FormStatus = ApplicationFormStatusEnum.AssessmentStage,
            Position = "null"
        };
        await context.ApplicationForms.AddAsync(form);
        await context.SaveChangesAsync();

        var result = await repo.DeleteApplicationFormById(form.Id);

        Assert.True(result);
        Assert.Equal(0, await context.ApplicationForms.CountAsync());
    }

    [Fact]
    public async Task GetMyApplicationForms_ShouldReturnPaginatedFilteredResults()
    {
        var context = GetDbContext();
        var paginationMock = new Mock<IPaginationBaseRepository<ApplicationForm, PagedApplicationFormRequest>>();
        var repo = new ApplicationFormRepository(context, paginationMock.Object);

        var userId = Guid.NewGuid();

        var forms = new List<ApplicationForm>
        {
            new()
            {
                Id = Guid.NewGuid(), UserId = userId, Company = "CompanyA", Position = "Developer",
                FormStatus = ApplicationFormStatusEnum.AssessmentStage, AppliedOn = new DateTime(2025, 1, 1)
            },
            new()
            {
                Id = Guid.NewGuid(), UserId = userId, Company = "CompanyB", Position = "Tester",
                FormStatus = ApplicationFormStatusEnum.InterviewStage, AppliedOn = new DateTime(2025, 1, 2)
            },
            new()
            {
                Id = Guid.NewGuid(), UserId = Guid.NewGuid(), Company = "OtherCompany", Position = "Developer",
                FormStatus = ApplicationFormStatusEnum.OfferStage, AppliedOn = new DateTime(2025, 1, 3)
            }
        };

        await context.ApplicationForms.AddRangeAsync(forms);
        await context.SaveChangesAsync();

        var pagedRequest = new PaginatedRequest<ApplicationForm, PagedApplicationFormRequest>
        {
            Model = new PagedApplicationFormRequest
            {
                UserId = userId,
                Company = "CompanyA",
                Position = "Developer",
                FormStatus = "AssessmentStage",
                AppliedOn = new DateTime(2025, 1, 1)
            }
        };

        paginationMock
            .Setup(p => p.GetPagedAsync(It.IsAny<PaginatedRequest<ApplicationForm, PagedApplicationFormRequest>>()))
            .ReturnsAsync((PaginatedRequest<ApplicationForm, PagedApplicationFormRequest> r) =>
            {
                var data = r.Query!.ToList();
                return new PaginatedResponse<ApplicationForm>
                {
                    Items = data,
                    TotalRecords = data.Count
                };
            });

        var result = await repo.GetMyApplicationForms(pagedRequest);

        Assert.NotNull(result);
        Assert.Equal("CompanyA", result.Items.First().Company);
        Assert.Equal(ApplicationFormStatusEnum.AssessmentStage, result.Items.First().FormStatus);
    }
}