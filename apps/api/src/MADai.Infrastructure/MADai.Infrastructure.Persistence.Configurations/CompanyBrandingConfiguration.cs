using MADai.Domain.Tenancy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MADai.Infrastructure.Persistence.Configurations;

public class CompanyBrandingConfiguration : IEntityTypeConfiguration<CompanyBranding>
{
	public void Configure(EntityTypeBuilder<CompanyBranding> builder)
	{
		builder.ToTable("CompanyBranding");
		builder.HasKey((CompanyBranding b) => b.Id);
		builder.Property((CompanyBranding b) => b.LogoUrl).HasMaxLength(500);
		builder.Property((CompanyBranding b) => b.FaviconUrl).HasMaxLength(500);
		builder.Property((CompanyBranding b) => b.PrimaryColor).HasMaxLength(20);
		builder.Property((CompanyBranding b) => b.AccentColor).HasMaxLength(20);
		builder.Property((CompanyBranding b) => b.BackgroundColor).HasMaxLength(20);
		builder.Property((CompanyBranding b) => b.ThemeMode).HasMaxLength(20);
		builder.Property((CompanyBranding b) => b.EmailFromName).HasMaxLength(120);
		builder.Property((CompanyBranding b) => b.EmailFromAddress).HasMaxLength(254);
	}
}
