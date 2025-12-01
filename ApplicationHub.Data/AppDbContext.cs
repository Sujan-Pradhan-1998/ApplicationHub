using ApplicationHub.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace ApplicationHub.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options, IDbOption dbOption): DbContext(options)
{
    private readonly string _connectionString = dbOption.GetSqlLiteConnectionString();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ApplicationConfig());
        modelBuilder.ApplyConfiguration(new ApplicationConfig());
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder
                .UseSqlite(_connectionString)
                .UseSnakeCaseNamingConvention();
        }
    }
    public void EnsureDatabaseCreated()
    {
        Database.EnsureCreated();
    }
}