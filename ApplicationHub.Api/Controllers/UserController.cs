using System.Net;
using ApplicationHub.Data.Services.IServices;
using ApplicationHub.Modules.Dtos.Requests;
using ApplicationHub.Modules.Dtos.Responses;
using ApplicationHub.Modules.Extensions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationHub.Api.Controllers;

/// <summary>
/// User Controller
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class UserController(IUserService userService, IValidator<UserRequest> userRequestValidator) : ControllerBase
{
    /// <summary>
    /// Get user by id
    /// </summary>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        if (id == Guid.Empty)
        {
            return BadRequest("Invalid Id.");
        }

        var user = await userService.GetUserId(id);

        if (user is null)
            return NotFound("User not found.");

        return Ok(user);
    }

    /// <summary>
    /// Add new user
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponse))]
    public async Task<IActionResult> Post([FromBody] UserRequest request)
    {
        ValidationResult validationResult = await userRequestValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return StatusCode((int)HttpStatusCode.BadRequest, ModelState);
        }
        
        //TODO Add user
        return Ok();
    }
}