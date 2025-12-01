using ApplicationHub.Data.Repos.IRepos;
using ApplicationHub.Modules.Dtos.Requests;
using ApplicationHub.Modules.Entities;
using ApplicationHub.Modules.Enums;
using ApplicationHub.Modules.Models;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace ApplicationHub.Data.Repos;

public class ApplicationFormRepository(
    AppDbContext context,
    IPaginationBaseRepository<ApplicationForm, PagedApplicationFormRequest> paginationBaseRepository)
    : IApplicationFormRepository
{
    public async Task<ApplicationForm?> GetApplicationFormById(Guid applicationFormId)
    {
        return await context.ApplicationForms.FirstOrDefaultAsync(x => x.Id == applicationFormId);
    }

    public async Task<ApplicationForm?> AddApplicationForm(ApplicationFormRequest applicationFormRequest)
    {
        var application = applicationFormRequest.Adapt<ApplicationForm>();
        application.FormStatus = ApplicationFormStatusEnum.ApplicationReceivedStage;
        await context.ApplicationForms.AddAsync(application);
        await context.SaveChangesAsync();
        return application;
    }

    public async Task<PaginatedResponse<ApplicationForm>> GetMyApplicationForms(
        PaginatedRequest<ApplicationForm, PagedApplicationFormRequest> pagedRequest)
    {
        var queryable = context.ApplicationForms.AsQueryable().AsNoTracking()
            .Where(x => x.UserId == pagedRequest.Model!.UserId);

        if (pagedRequest.Model!.FormStatus is not null &&
            pagedRequest.Model!.FormStatus >= ApplicationFormStatusEnum.ApplicationReceivedStage)
        {
            queryable = queryable.Where(x => x.FormStatus == pagedRequest.Model.FormStatus);
        }

        if (!string.IsNullOrWhiteSpace(pagedRequest.Model.Company))
        {
            queryable = queryable.Where(x =>
                x.Company.ToLower().Contains(pagedRequest.Model.Company.ToLower()));
        }

        if (!string.IsNullOrWhiteSpace(pagedRequest.Model.Position))
        {
            queryable = queryable.Where(x =>
                x.Position.ToLower().Contains(pagedRequest.Model.Position.ToLower()));
        }

        if (pagedRequest.Model.AppliedOn is not null)
        {
            queryable = queryable.Where(x =>
                x.AppliedOn == pagedRequest.Model.AppliedOn);
        }

        pagedRequest.Query = queryable;
        var data = await paginationBaseRepository.GetPagedAsync(pagedRequest);
        return data;
    }
}