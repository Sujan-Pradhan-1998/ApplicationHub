using ApplicationHub.Modules.Dtos.Requests;
using ApplicationHub.Modules.Entities;
using ApplicationHub.Modules.Models;

namespace ApplicationHub.Data.Repos.IRepos;

public interface IApplicationFormRepository
{
    public Task<ApplicationForm?> GetApplicationFormById(Guid applicationFormId);
    public Task<ApplicationForm?> AddApplicationForm(ApplicationFormRequest applicationFormRequest);
    public Task<ApplicationForm?> UpdateApplicationForm(ApplicationFormUpdateRequest applicationFormUpdateRequest);

    public Task<PaginatedResponse<ApplicationForm>> GetMyApplicationForms(
        PaginatedRequest<ApplicationForm, PagedApplicationFormRequest> pagedRequest);
    public Task<bool> DeleteApplicationFormById(Guid applicationFormId);
}