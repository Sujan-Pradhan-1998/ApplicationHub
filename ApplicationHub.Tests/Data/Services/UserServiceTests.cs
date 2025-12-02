using ApplicationHub.Data.Repos.IRepos;
using ApplicationHub.Data.Services;
using ApplicationHub.Modules.Dtos.Requests;
using ApplicationHub.Modules.Entities;
using ApplicationHub.Modules.Helpers;
using Microsoft.Extensions.Logging;
using Moq;

namespace ApplicationHub.Tests.Data.Services;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<ILogger<UserService>> _loggerMock;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _loggerMock = new Mock<ILogger<UserService>>();
        _userService = new UserService(_userRepositoryMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task GetUserById_ReturnsUserResponse_WhenUserExists()
    {
        var userId = Guid.NewGuid();
        var user = new User
        {
            Id = userId,
            Email = "test@example.com",
            Password = "hashed",
            FirstName = "test",
            LastName = "test"
        };
        _userRepositoryMock.Setup(x => x.GetUserById(userId)).ReturnsAsync(user);

        var result = await _userService.GetUserById(userId);

        Assert.NotNull(result);
        Assert.Equal(userId, result!.Id);
        Assert.Equal(user.Email, result.Email);
    }

    [Fact]
    public async Task GetUserById_ReturnsNull_WhenUserDoesNotExist()
    {
        _userRepositoryMock.Setup(x => x.GetUserById(It.IsAny<Guid>())).ReturnsAsync((User?)null);

        var result = await _userService.GetUserById(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public async Task AddUser_ReturnsUserResponse()
    {
        var request = new UserRequest
        {
            Email = "test@example.com",
            Password = "hashed",
            FirstName = "test",
            LastName = "test"
        };
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = request.Email,
            Password = PasswordHelper.CreateHash(request.Password,
                request.Email),
            FirstName = "test",
            LastName = "test"
        };
        _userRepositoryMock.Setup(x => x.AddUser(It.IsAny<User>())).ReturnsAsync(user);

        var result = await _userService.AddUser(request);

        Assert.NotNull(result);
        Assert.Equal(user.Id, result!.Id);
        Assert.Equal(user.Email, result.Email);
    }

    [Fact]
    public async Task Login_ReturnsUserResponse_WhenCredentialsAreValid()
    {
        var loginRequest = new LoginRequest { Email = "test@example.com", Password = "password" };
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = loginRequest.Email,
            Password = PasswordHelper.CreateHash(loginRequest.Password,
                loginRequest.Email),
            FirstName = "test",
            LastName = "test"
        };
        _userRepositoryMock.Setup(x => x.GetUserByEmail(loginRequest.Email)).ReturnsAsync(user);
        _userRepositoryMock.Setup(x => x.UpdateLastLogin(user.Id))
            .Returns(Task.FromResult(true));


        var result = await _userService.Login(loginRequest);

        Assert.NotNull(result);
        Assert.Equal(user.Id, result!.Id);
        Assert.Equal(user.Email, result.Email);
    }

    [Fact]
    public async Task Login_ReturnsNull_WhenUserDoesNotExist()
    {
        _userRepositoryMock.Setup(x => x.GetUserByEmail(It.IsAny<string>())).ReturnsAsync((User?)null);

        var result = await _userService.Login(new LoginRequest
            { Email = "nonexistent@example.com", Password = "pass" });

        Assert.Null(result);
    }

    [Fact]
    public async Task Login_ReturnsNull_WhenPasswordIsInvalid()
    {
        var loginRequest = new LoginRequest { Email = "test@example.com", Password = "wrongpass" };
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = loginRequest.Email,
            Password = PasswordHelper.CreateHash("correctpass",
                loginRequest.Email),
            FirstName = "test",
            LastName = "test"
        };
        _userRepositoryMock.Setup(x => x.GetUserByEmail(loginRequest.Email)).ReturnsAsync(user);

        var result = await _userService.Login(loginRequest);

        Assert.Null(result);
        _loggerMock.Verify(x => x.Log(
            It.IsAny<Microsoft.Extensions.Logging.LogLevel>(),
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains(loginRequest.Email)),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);
    }
}