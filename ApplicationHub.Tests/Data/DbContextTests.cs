using ApplicationHub.Data;
using ApplicationHub.Modules.Entities;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace ApplicationHub.Tests.Data;

public class AppDbContextTests
{
    private AppDbContext CreateContext()
    {
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(connection)
            .Options;

        var dbOption = new TestDbOption(connection.ConnectionString);

        var context = new AppDbContext(options, dbOption);
        context.Database.EnsureCreated();
        return context;
    }

    [Fact]
    public void SaveChanges_ShouldSetCreatedOn_ForUser()
    {
        using var context = CreateContext();

        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = "Test",
            LastName = "Test",
            Email = "test@test.com",
            Password = "password",
        };
        context.Users.Add(user);
        context.SaveChanges();

        Assert.True(user.CreatedOn != default);
        Assert.InRange(user.CreatedOn, DateTime.UtcNow.AddMinutes(-1), DateTime.UtcNow.AddMinutes(1));
    }

    [Fact]
    public async Task SaveChangesAsync_ShouldSetCreatedOn_ForUser()
    {
        using var context = CreateContext();

        var user = new User
        {
            Id = Guid.NewGuid(),
            FirstName = "Test",
            LastName = "Test",
            Email = "test@test.com",
            Password = "password",
        };
        context.Users.Add(user);
        await context.SaveChangesAsync();

        Assert.True(user.CreatedOn != default);
        Assert.InRange(user.CreatedOn, DateTime.UtcNow.AddMinutes(-1), DateTime.UtcNow.AddMinutes(1));
    }
}

public class TestDbOption : IDbOption
{
    private readonly string _connectionString;
    public TestDbOption(string connectionString) => _connectionString = connectionString;
    public string GetSqlLiteConnectionString() => _connectionString;
}