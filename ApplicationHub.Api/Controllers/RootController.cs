using ApplicationHub.Modules.Constants;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationHub.Api.Controllers;

/// <summary>
/// Application Hub Root
/// </summary>
/// <param name="configuration"></param>
[ApiController]
[Route("api/[controller]")]
public class RootController(IConfiguration configuration) : ControllerBase
{
    /// <summary>
    /// Get Application Hub version information
    /// </summary>
    /// <returns>Returns the current version of the Application Hub.</returns>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [HttpGet("version")]
    public string? Get()
    {
        return configuration[ApplicationHubConstants.Version];
    }
}