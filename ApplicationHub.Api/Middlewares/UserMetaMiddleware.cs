using System.IdentityModel.Tokens.Jwt;

namespace ApplicationHub.Api.Middlewares;

/// <summary>
/// Middleware that globally handles userMeta in the request pipeline.
/// </summary>
/// <param name="next"></param>
public class UserMetaMiddleware(RequestDelegate next)
{
    /// <summary>
    /// Invokes the middleware and handles userMeta in the request pipeline.
    /// </summary>
    /// <param name="context"></param>
    /// <param name="userMeta"></param>
    public async Task InvokeAsync(HttpContext context, IUserMeta userMeta)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        if (!string.IsNullOrEmpty(token))
        {
            var handler = new JwtSecurityTokenHandler();
            var jwt = handler.ReadJwtToken(token);

            userMeta.CurrentCompany = jwt.Claims.FirstOrDefault(c => c.Type == "currentCompany")?.Value ?? "";
            userMeta.Email = jwt.Claims.FirstOrDefault(c => c.Type == "email")?.Value ?? "";
            userMeta.Id = Guid.Parse(jwt.Claims.FirstOrDefault(c => c.Type == "id")?.Value ?? "");
            userMeta.UserName = jwt.Claims.FirstOrDefault(c => c.Type == "userName")?.Value ?? "";
        }

        await next(context);
    }
}