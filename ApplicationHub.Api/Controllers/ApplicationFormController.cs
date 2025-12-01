using System.Text.Json;
using ApplicationHub.Data.Services.IServices;
using ApplicationHub.Modules.Dtos.Requests;
using ApplicationHub.Modules.Dtos.Responses;
using ApplicationHub.Modules.Entities;
using ApplicationHub.Modules.Extensions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationHub.Api.Controllers;

/// <summary>
/// Application Tracking API
/// </summary>
[Authorize]
[Route("api/applicationform")]
public class ApplicationFormController(
    IApplicationFormService applicationFormService,
    IUserMeta userMeta,
    IValidator<ApplicationFormRequest> applicationFormRequestValidator,
    IValidator<PaginatedRequest<ApplicationForm, PagedApplicationFormRequest>> pagedRequestValidator
) : ControllerBase
{
    /// <summary>
    /// Get application form by id
    /// </summary>
    /// <param name="applicationFormId"></param>
    /// <returns></returns>
    [HttpGet("{applicationFormId:guid}")]
    public async Task<IActionResult> GetApplicationFormById(Guid applicationFormId)
    {
        var applicationForm = await applicationFormService.GetApplicationFormById(applicationFormId);
        if (applicationForm is null) return NotFound("Application Form not found.");
        return Ok(applicationForm);
    }

    /// <summary>
    /// Get login user application form
    /// </summary>
    /// <returns></returns>
    [HttpGet("getmyapplicationforms")]
    public async Task<IActionResult> GetMyApplicationForms(
        PaginatedRequest<ApplicationForm, PagedApplicationFormRequest> pageRequest)
    {
        if (pageRequest.Model is null) pageRequest.Model = new PagedApplicationFormRequest();
        
        pageRequest.Model.UserId = userMeta.Id!.Value;
        ValidationResult validationResult = await pagedRequestValidator.ValidateAsync(pageRequest);
        if (!validationResult.IsValid)
        {
            validationResult.AddErrorsToModelState(ModelState);
            return BadRequest(ModelState);
        }

        var res = await applicationFormService.GetMyApplicationForms(pageRequest);
        return Ok(res);
    }

    /// <summary>
    /// Add new application form
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ApplicationFormResponse))]
    public async Task<IActionResult> Post([FromBody] ApplicationFormRequest request)
    {
        request.UserId = (Guid)userMeta.Id!;

        ValidationResult validationResult = await applicationFormRequestValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            validationResult.AddErrorsToModelState(ModelState);
            return BadRequest(ModelState);
        }

        var applicationForm = await applicationFormService.AddApplicationForm(request);
        if (applicationForm is null) return BadRequest("Unable to add application form.");
        return Ok(applicationForm);
    }
}