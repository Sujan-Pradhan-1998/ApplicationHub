using ApplicationHub.Data;
using ApplicationHub.Data.Repos;
using ApplicationHub.Modules.Entities;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace ApplicationHub.Tests.Data.Repos;

public class UserRepositoryTests
{
    private AppDbContext GetInMemoryDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        var mockDbOption = new Mock<IDbOption>();
        return new AppDbContext(options, mockDbOption.Object);
    }

    [Fact]
    public async Task AddUser_Should_Add_User()
    {
        var context = GetInMemoryDbContext();
        var repo = new UserRepository(context);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "TEST@EMAIL.COM",
            FirstName = "test",
            LastName = "null",
            Password = "null"
        };
        var addedUser = await repo.AddUser(user);

        Assert.NotNull(addedUser);
        Assert.Equal("test@email.com", addedUser.Email);
        Assert.Single(context.Users);
    }

    [Fact]
    public async Task GetUserById_Should_Return_User_When_Exists()
    {
        var context = GetInMemoryDbContext();
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "user@example.com",
            FirstName = "null",
            LastName = "null",
            Password = "null"
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var repo = new UserRepository(context);
        var result = await repo.GetUserById(user.Id);

        Assert.NotNull(result);
        Assert.Equal(user.Id, result.Id);
    }

    [Fact]
    public async Task GetUserByEmail_Should_Return_User_When_Exists()
    {
        var context = GetInMemoryDbContext();
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "user@example.com",
            FirstName = "null",
            LastName = "null",
            Password = "null"
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var repo = new UserRepository(context);
        var result = await repo.GetUserByEmail("USER@EXAMPLE.COM");

        Assert.NotNull(result);
        Assert.Equal(user.Email, result.Email);
    }

    [Fact]
    public async Task UpdateLastLogin_Should_Update_If_User_Exists()
    {
        var context = GetInMemoryDbContext();
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "user@example.com",
            FirstName = "null",
            LastName = "null",
            Password = "null"
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var repo = new UserRepository(context);
        var result = await repo.UpdateLastLogin(user.Id);

        Assert.True(result);
        var updatedUser = await context.Users.FirstOrDefaultAsync(x => x.Id == user.Id);
        Assert.NotNull(updatedUser?.LastLoginTime);
    }

    [Fact]
    public async Task UpdateLastLogin_Should_Return_False_If_User_Does_Not_Exist()
    {
        var context = GetInMemoryDbContext();
        var repo = new UserRepository(context);
        var result = await repo.UpdateLastLogin(Guid.NewGuid());

        Assert.False(result);
    }
}
