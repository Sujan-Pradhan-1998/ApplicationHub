using ApplicationHub.Api.Controllers;
using ApplicationHub.Data.Services.IServices;
using ApplicationHub.Modules.Dtos.Requests;
using ApplicationHub.Modules.Dtos.Responses;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace ApplicationHub.Tests.Controllers;

public class LoginControllerTests
{
    // Helper to create a controller with a mock HttpContext + auth services
    private LoginController CreateController(
        IUserService userService,
        IValidator<LoginRequest> validator,
        IConfiguration config)
    {
        var controller = new LoginController(userService, validator, config);

        var httpContext = new DefaultHttpContext();

        // Register minimal authentication services to support SignInAsync/SignOutAsync
        var services = new ServiceCollection();
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme);
        services.AddLogging();

        httpContext.RequestServices = services.BuildServiceProvider();

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        return controller;
    }

    [Fact]
    public async Task Login_ReturnsOk_WithJwtToken()
    {
        // Arrange
        var userService = new Mock<IUserService>();
        var validator = new Mock<IValidator<LoginRequest>>();

        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                { "JwtKey", "ThisIsASuperSecretTestKeyForJWT123456!" }
            }!)
            .Build();

        var request = new LoginRequest { Email = "test@example.com", Password = "password" };

        validator
            .Setup(v => v.ValidateAsync(request, default))
            .ReturnsAsync(new ValidationResult());

        // Fake user returned by IUserService.Login
        userService
            .Setup(s => s.Login(request))
            .ReturnsAsync(new UserResponse()
            {
                Id = Guid.NewGuid(),
                FirstName = "John",
                MiddleName = "",
                LastName = "Doe",
                Email = "test@example.com",
                CurrentCompany = "Acme"
            });

        var controller = CreateController(userService.Object, validator.Object, config);

        // Act
        var result = await controller.Login(request);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var token = Assert.IsType<string>(okResult.Value);

        Assert.False(string.IsNullOrWhiteSpace(token));
        Assert.Contains(".", token); // JWT format
    }

    [Fact]
    public async Task Login_InvalidModel_ReturnsBadRequest()
    {
        // Arrange
        var userService = new Mock<IUserService>();
        var validator = new Mock<IValidator<LoginRequest>>();

        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string> { { "JwtKey", "Key" } }!)
            .Build();

        var request = new LoginRequest()
        {
            Email = "test@example.com",
            Password = "password",
        };

        var validationResult = new ValidationResult(new[]
        {
            new ValidationFailure("Email", "Required")
        });

        validator
            .Setup(v => v.ValidateAsync(request, default))
            .ReturnsAsync(validationResult);

        var controller = CreateController(userService.Object, validator.Object, config);

        // Act
        var result = await controller.Login(request);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Login_InvalidCredentials_ReturnsUnauthorized()
    {
        // Arrange
        var userService = new Mock<IUserService>();
        var validator = new Mock<IValidator<LoginRequest>>();

        var config = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string> { { "JwtKey", "Key" } }!)
            .Build();

        var request = new LoginRequest { Email = "test@test.com", Password = "wrong" };

        validator
            .Setup(v => v.ValidateAsync(request, default))
            .ReturnsAsync(new ValidationResult());

        userService
            .Setup(s => s.Login(request))
            .ReturnsAsync((UserResponse?)null); // simulate user not found

        var controller = CreateController(userService.Object, validator.Object, config);

        // Act
        var result = await controller.Login(request);

        // Assert
        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public async Task Logout_ReturnsOk()
    {
        // Arrange
        var userService = new Mock<IUserService>();
        var validator = new Mock<IValidator<LoginRequest>>();

        var config = new ConfigurationBuilder().Build();

        var controller = CreateController(userService.Object, validator.Object, config);

        // Act
        var result = await controller.Logout();

        // Assert
        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, ok.StatusCode);
    }
}