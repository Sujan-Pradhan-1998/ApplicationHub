using ApplicationHub.Api;
using ApplicationHub.Api.Controllers;
using ApplicationHub.Data.Services.IServices;
using ApplicationHub.Modules.Dtos.Requests;
using ApplicationHub.Modules.Dtos.Responses;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ApplicationHub.Tests.Controllers;

public class UserControllerTests
{
    private readonly Mock<IUserService> _userServiceMock;
    private readonly Mock<IValidator<UserRequest>> _validatorMock;
    private readonly UserController _controller;

    public UserControllerTests()
    {
        _userServiceMock = new Mock<IUserService>();
        _validatorMock = new Mock<IValidator<UserRequest>>();
        Mock<IUserMeta> userMetaMock = new Mock<IUserMeta>();
        _controller = new UserController(_userServiceMock.Object, _validatorMock.Object, userMetaMock.Object);
    }

    [Fact]
    public async Task Get_ReturnsBadRequest_WhenIdIsEmpty()
    {
        var result = await _controller.Get(Guid.Empty);
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Get_ReturnsNotFound_WhenUserNotFound()
    {
        _userServiceMock.Setup(s => s.GetUserById(It.IsAny<Guid>())).ReturnsAsync((UserResponse)null!);

        var result = await _controller.Get(Guid.NewGuid());

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task Get_ReturnsOk_WhenUserExists()
    {
        var user = new UserResponse
        {
            Id = Guid.NewGuid(),
            Email = "test@test.com",
            FirstName = "Test",
            LastName = "Test",
            CurrentCompany = "Test",
            MiddleName = "Test",
            CreatedOn = DateTime.Now,
            LastLoginTime = DateTime.Now,
        };
        _userServiceMock.Setup(s => s.GetUserById(It.IsAny<Guid>())).ReturnsAsync(user);

        var result = await _controller.Get(Guid.NewGuid());

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(user, okResult.Value);
    }

    [Fact]
    public async Task RegisterUser_ReturnsBadRequest_WhenValidationFails()
    {
        var request = new UserRequest()
        {
            Email = "test@test.com",
            FirstName = "Test",
            LastName = "Test",
            CurrentCompany = "Test",
            MiddleName = "Test",
            Password = "password",
        };
        _validatorMock.Setup(v => v.ValidateAsync(request, CancellationToken.None))
            .ReturnsAsync(new ValidationResult([new ValidationFailure("Name", "Required")]));

        var result = await _controller.RegisterUser(request);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task RegisterUser_ReturnsBadRequest_WhenServiceReturnsNull()
    {
        var request = new UserRequest()
        {
            Email = "test@test.com",
            FirstName = "Test",
            LastName = "Test",
            CurrentCompany = "Test",
            MiddleName = "Test",
            Password = "password",
        };
        _validatorMock.Setup(v => v.ValidateAsync(request, CancellationToken.None))
            .ReturnsAsync(new ValidationResult());
        _userServiceMock.Setup(s => s.AddUser(request)).ReturnsAsync((UserResponse)null!);

        var result = await _controller.RegisterUser(request);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task RegisterUser_ReturnsOk_WhenUserCreated()
    {
        var request = new UserRequest()
        {
            Email = "test@test.com",
            FirstName = "Test",
            LastName = "Test",
            CurrentCompany = "Test",
            MiddleName = "Test",
            Password = "password",
        };
        var newUser = new UserResponse
        {
            Id = Guid.NewGuid(),
            Email = "test@test.com",
            FirstName = "Test",
            LastName = "Test",
            CurrentCompany = "Test",
            MiddleName = "Test",
            CreatedOn = DateTime.Now,
            LastLoginTime = DateTime.Now,
        };

        _validatorMock.Setup(v => v.ValidateAsync(request, CancellationToken.None))
            .ReturnsAsync(new ValidationResult());
        _userServiceMock.Setup(s => s.AddUser(request)).ReturnsAsync(newUser);

        var result = await _controller.RegisterUser(request);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(newUser, okResult.Value);
    }

    [Fact]
    public void GetUserMeta_ReturnsOk()
    {
        var meta = new Mock<IUserMeta>();
        var controller = new UserController(_userServiceMock.Object, _validatorMock.Object, meta.Object);

        var result = controller.GetUserMeta();

        Assert.IsType<OkObjectResult>(result);
    }
}