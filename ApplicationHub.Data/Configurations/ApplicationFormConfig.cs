using ApplicationHub.Modules.Entities;
using ApplicationHub.Modules.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ApplicationHub.Data.Configurations;

public class ApplicationFormConfig : IEntityTypeConfiguration<ApplicationForm>
{
    public void Configure(EntityTypeBuilder<ApplicationForm> builder)
    {
        builder.ToTable("application_forms");
        builder.HasKey("Id");
        builder.Property(x => x.FormStatus).HasDefaultValue(ApplicationFormStatusEnum.ApplicationReceivedStage);
    }
}