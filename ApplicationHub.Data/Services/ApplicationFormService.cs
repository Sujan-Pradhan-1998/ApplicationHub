using ApplicationHub.Data.Repos.IRepos;
using ApplicationHub.Data.Services.IServices;
using ApplicationHub.Modules.Dtos.Requests;
using ApplicationHub.Modules.Dtos.Responses;
using ApplicationHub.Modules.Entities;
using ApplicationHub.Modules.Models;
using Mapster;

namespace ApplicationHub.Data.Services;

public class ApplicationFormService(
    IApplicationFormRepository applicationFormRepository) : IApplicationFormService
{
    public async Task<ApplicationFormResponse?> GetApplicationFormById(Guid applicationFormId)
    {
        var application = await applicationFormRepository.GetApplicationFormById(applicationFormId);
        return application?.Adapt<ApplicationFormResponse>();
    }

    public async Task<ApplicationFormResponse?> AddApplicationForm(ApplicationFormRequest applicationFormRequest)
    {
        var application = await applicationFormRepository.AddApplicationForm(applicationFormRequest);
        return application?.Adapt<ApplicationFormResponse>();
    }

    public async Task<ApplicationFormResponse?> UpdateApplicationForm(ApplicationFormUpdateRequest applicationFormUpdateRequest)
    {
        var application = await applicationFormRepository.UpdateApplicationForm(applicationFormUpdateRequest);
        return application?.Adapt<ApplicationFormResponse>();
    }

    public async Task<PaginatedResponse<ApplicationFormResponse>> GetMyApplicationForms(
        PaginatedRequest<ApplicationForm, PagedApplicationFormRequest> pagedRequest)
    {
        var data = await applicationFormRepository.GetMyApplicationForms(pagedRequest);
        return data.Adapt<PaginatedResponse<ApplicationFormResponse>>();
    }

    public async Task<bool> DeleteApplicationFormById(Guid applicationFormId)
    {
        return await applicationFormRepository.DeleteApplicationFormById(applicationFormId);
    }
}