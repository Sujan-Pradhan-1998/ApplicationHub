using ApplicationHub.Data;
using Microsoft.Extensions.Configuration;

namespace ApplicationHub.Tests.Data;

public class DbOptionTests
{
    [Fact]
    public void GetSqlLiteConnectionString_ReturnsExpectedConnectionString()
    {
        var inMemorySettings = new Dictionary<string, string>
        {
            { "DbServer:Database", "testdb" }
        };

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings!)
            .Build();

        var dbOption = new DbOption(configuration);

        var connectionString = dbOption.GetSqlLiteConnectionString();

        Assert.Equal("Data Source=testdb", connectionString);
    }

    [Fact]
    public void GetSqlLiteConnectionString_UsesDefaultWhenConfigMissing()
    {
        IConfiguration configuration = new ConfigurationBuilder().Build();

        var dbOption = new DbOption(configuration);

        var connectionString = dbOption.GetSqlLiteConnectionString();

        Assert.Equal("Data Source=candidate", connectionString);
    }
}