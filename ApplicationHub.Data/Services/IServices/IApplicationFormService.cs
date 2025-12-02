using ApplicationHub.Modules.Dtos.Requests;
using ApplicationHub.Modules.Dtos.Responses;
using ApplicationHub.Modules.Entities;
using ApplicationHub.Modules.Models;

namespace ApplicationHub.Data.Services.IServices;

public interface IApplicationFormService
{
    public Task<ApplicationFormResponse?> GetApplicationFormById(Guid applicationFormId);
    public Task<ApplicationFormResponse?> AddApplicationForm(ApplicationFormRequest applicationFormRequest);
    public Task<ApplicationFormResponse?> UpdateApplicationForm(ApplicationFormUpdateRequest applicationFormUpdateRequest);

    public Task<PaginatedResponse<ApplicationFormResponse>> GetMyApplicationForms(
        PaginatedRequest<ApplicationForm, PagedApplicationFormRequest> pagedRequest);
    
    public Task<bool> DeleteApplicationFormById(Guid applicationFormId);
}