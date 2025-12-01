using ApplicationHub.Data.Services.IServices;
using ApplicationHub.Modules.Dtos.Requests;
using ApplicationHub.Modules.Dtos.Responses;
using ApplicationHub.Modules.Extensions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationHub.Api.Controllers;

/// <summary>
/// User Controller
/// </summary>
[ApiController]
[Route("api/user")]
[Authorize]
public class UserController(IUserService userService, IValidator<UserRequest> userRequestValidator, IUserMeta userMeta) : ControllerBase
{
    /// <summary>
    /// Get user by id
    /// </summary>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        if (id == Guid.Empty) return BadRequest("Invalid Id.");
        var user = await userService.GetUserById(id);

        if (user is null)
            return NotFound("User not found.");

        return Ok(user);
    }

    /// <summary>
    /// Register new user for Application Hub
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("registeruser")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponse))]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterUser([FromBody] UserRequest request)
    {
        ValidationResult validationResult = await userRequestValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            validationResult.AddErrorsToModelState(ModelState);
            return BadRequest(ModelState);
        }

        var newUser = await userService.AddUser(request);
        if (newUser is null) return BadRequest("Unable to create user.");
        return Ok(newUser);
    }

    /// <summary>
    /// Get current user meta
    /// </summary>
    /// <returns></returns>
    [HttpGet("usermeta")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserMeta))]
    public IActionResult GetUserMeta()
    {
        return Ok(userMeta);
    }
}