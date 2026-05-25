using MADai.Domain.Tenancy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MADai.Infrastructure.Persistence.Configurations;

public class CompanyConfiguration : IEntityTypeConfiguration<Company>
{
	public void Configure(EntityTypeBuilder<Company> builder)
	{
		builder.ToTable("Companies");
		builder.HasKey((Company c) => c.Id);
		builder.Property((Company c) => c.Name).IsRequired().HasMaxLength(200);
		builder.Property((Company c) => c.Slug).IsRequired().HasMaxLength(80);
		builder.Property((Company c) => c.LegalName).HasMaxLength(200);
		builder.Property((Company c) => c.ContactEmail).HasMaxLength(254);
		builder.Property((Company c) => c.ContactPhone).HasMaxLength(40);
		builder.Property((Company c) => c.Website).HasMaxLength(200);
		builder.Property((Company c) => c.Country).HasMaxLength(80);
		builder.Property((Company c) => c.Timezone).HasMaxLength(80);
		builder.HasIndex((Company c) => c.Slug).IsUnique();
		builder.HasOne((Company c) => c.Branding).WithOne((CompanyBranding b) => b.Company).HasForeignKey((CompanyBranding b) => b.CompanyId)
			.OnDelete(DeleteBehavior.Cascade);
		builder.HasOne((Company c) => c.Settings).WithOne((CompanySettings s) => s.Company).HasForeignKey((CompanySettings s) => s.CompanyId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}
