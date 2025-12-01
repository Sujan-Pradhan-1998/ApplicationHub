using ApplicationHub.Modules.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApplicationHub.Data.Configurations;

public class ApplicationConfig : IEntityTypeConfiguration<Application>
{
    public void Configure(EntityTypeBuilder<Application> builder)
    {
        builder.ToTable("applications");
        builder.HasKey("Id");
    }
}