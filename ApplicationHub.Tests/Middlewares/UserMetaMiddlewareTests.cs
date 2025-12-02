using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ApplicationHub.Api;
using ApplicationHub.Api.Middlewares;
using Microsoft.AspNetCore.Http;

namespace ApplicationHub.Tests.Middlewares
{
    public class UserMetaMiddlewareTests
    {
        private class TestUserMeta : IUserMeta
        {
            public string CurrentCompany { get; set; } = "";
            public string Email { get; set; } = "";
            public Guid? Id { get; set; } = Guid.Empty;
            public string UserName { get; set; } = "";
        }

        [Fact]
        public async Task InvokeAsync_ShouldPopulateUserMeta_WhenTokenIsValid()
        {
            var claims = new[]
            {
                new Claim("currentCompany", "TestCompany"),
                new Claim("email", "test@example.com"),
                new Claim("id", Guid.NewGuid().ToString()),
                new Claim("userName", "tester")
            };
            var token = new JwtSecurityToken(claims: claims);
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            var context = new DefaultHttpContext();
            context.Request.Headers["Authorization"] = $"Bearer {tokenString}";

            var userMeta = new TestUserMeta();

            var middleware = new UserMetaMiddleware(Next);
            await middleware.InvokeAsync(context, userMeta);

            Assert.Equal("TestCompany", userMeta.CurrentCompany);
            Assert.Equal("test@example.com", userMeta.Email);
            Assert.Equal(claims.First(c => c.Type == "id").Value, userMeta.Id.ToString());
            Assert.Equal("tester", userMeta.UserName);
            return;

            Task Next(HttpContext ctx) => Task.CompletedTask;
        }

        [Fact]
        public async Task InvokeAsync_ShouldNotThrow_WhenNoToken()
        {
            var context = new DefaultHttpContext();
            var userMeta = new TestUserMeta();
            RequestDelegate next = (HttpContext ctx) => Task.CompletedTask;

            var middleware = new UserMetaMiddleware(next);
            var exception = await Record.ExceptionAsync(() => middleware.InvokeAsync(context, userMeta));

            Assert.Null(exception);
            Assert.Equal("", userMeta.CurrentCompany);
            Assert.Equal("", userMeta.Email);
            Assert.Equal(Guid.Empty, userMeta.Id);
            Assert.Equal("", userMeta.UserName);
        }
    }
}
