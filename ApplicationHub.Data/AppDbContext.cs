using ApplicationHub.Data.Configurations;
using ApplicationHub.Modules.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApplicationHub.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options, IDbOption dbOption) : DbContext(options)
{
    private readonly string _connectionString = dbOption.GetSqlLiteConnectionString();
    public DbSet<User> Users { get; set; }
    public DbSet<ApplicationForm> ApplicationForms { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ApplicationFormConfig());
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
    
    public override int SaveChanges()
    {
        SetCreatedOnTimestamps();
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        SetCreatedOnTimestamps();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void SetCreatedOnTimestamps()
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.State == EntityState.Added);

        foreach (var entry in entries)
        {
            var createdOnProp = entry.Metadata.FindProperty("CreatedOn");
            if (createdOnProp == null)
                continue;

            if (createdOnProp.ClrType == typeof(DateTime))
            {
                entry.Property("CreatedOn").CurrentValue = DateTime.UtcNow;
            }
            else if (createdOnProp.ClrType == typeof(DateOnly))
            {
                entry.Property("CreatedOn").CurrentValue = DateOnly.FromDateTime(DateTime.UtcNow);
            }
        }
    }
}