using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;

namespace ApplicationHub.Data;

public class DbOption(IConfiguration configuration) : IDbOption
{
    private string? Database { get; } = configuration.GetSection($"DbServer:Database").Value ?? "candidate";
    public string GetSqlLiteConnectionString()
    {
        var builder = new SqliteConnectionStringBuilder()
        {
            DataSource = Database,
        };
        return builder.ConnectionString;
    }
}