using MADai.Domain.Tenancy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MADai.Infrastructure.Persistence.Configurations;

public class CompanySettingsConfiguration : IEntityTypeConfiguration<CompanySettings>
{
	public void Configure(EntityTypeBuilder<CompanySettings> builder)
	{
		builder.ToTable("CompanySettings");
		builder.HasKey((CompanySettings s) => s.Id);
		builder.Property((CompanySettings s) => s.CustomDomain).HasMaxLength(200);
		builder.Property((CompanySettings s) => s.WebhookSecret).HasMaxLength(128);
	}
}
