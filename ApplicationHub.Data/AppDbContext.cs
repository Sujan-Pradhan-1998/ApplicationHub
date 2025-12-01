using ApplicationHub.Data.Configurations;
using ApplicationHub.Modules.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApplicationHub.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options, IDbOption dbOption) : DbContext(options)
{
    private readonly string _connectionString = dbOption.GetSqlLiteConnectionString();
    public DbSet<User> Users { get; set; }
    public DbSet<Application> Applications { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ApplicationConfig());
        modelBuilder.ApplyConfiguration(new UserConfig());
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