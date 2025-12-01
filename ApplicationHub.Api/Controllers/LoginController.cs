using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ApplicationHub.Data.Services.IServices;
using ApplicationHub.Modules.Dtos.Requests;
using ApplicationHub.Modules.Extensions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ApplicationHub.Api.Controllers;

/// <summary>
/// Login
/// </summary>
/// <param name="userService"></param>
/// <param name="loginValidator"></param>
/// <param name="configuration">Application configuration used to access JWT settings</param>
[ApiController]
[Route("api/login")]
public class LoginController(
    IUserService userService,
    IValidator<LoginRequest> loginValidator,
    IConfiguration configuration) : ControllerBase
{
    private readonly string _signingKey = configuration["JwtKey"]!;

    /// <summary>
    /// Login User 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [HttpPost]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        ValidationResult validationResult = await loginValidator.ValidateAsync(request);
        if (!validationResult.IsValid)
        {
            validationResult.AddErrorsToModelState(ModelState);
            return BadRequest(ModelState);
        }

        var loginUser = await userService.Login(request);
        if (loginUser is null) return Unauthorized();

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_signingKey);

        var fullName = loginUser.FirstName;

        if (string.IsNullOrWhiteSpace(loginUser.MiddleName)) fullName += " " + loginUser.MiddleName;
        fullName += " " + loginUser.LastName;

        var claims = new[]
        {
            new Claim("id", loginUser.Id.ToString()),
            new Claim(ClaimTypes.Email, loginUser.Email),
            new Claim("currentCompany", loginUser.CurrentCompany ?? string.Empty),
            new Claim("userName", fullName ?? string.Empty)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(10),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var bearerToken = tokenHandler.WriteToken(token);

        var identity = new ClaimsIdentity(claims, "Basic");
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity), new AuthenticationProperties
            {
                ExpiresUtc = DateTime.Now.AddHours(10),
                IsPersistent = true
            }).ConfigureAwait(true);

        return Ok(bearerToken);
    }

    /// <summary>
    /// Logout User
    /// </summary>
    /// <returns></returns>
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Ok(new { message = "Logged out" });
    }
}