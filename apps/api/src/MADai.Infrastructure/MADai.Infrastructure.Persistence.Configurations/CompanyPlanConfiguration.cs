using MADai.Domain.Tenancy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MADai.Infrastructure.Persistence.Configurations;

public class CompanyPlanConfiguration : IEntityTypeConfiguration<CompanyPlan>
{
	public void Configure(EntityTypeBuilder<CompanyPlan> builder)
	{
		builder.ToTable("CompanyPlans");
		builder.HasKey((CompanyPlan p) => p.Id);
		builder.Property((CompanyPlan p) => p.Name).IsRequired().HasMaxLength(120);
		builder.Property((CompanyPlan p) => p.Code).IsRequired().HasMaxLength(40);
		builder.Property((CompanyPlan p) => p.MonthlyPrice).HasPrecision(18, 2);
		builder.Property((CompanyPlan p) => p.AnnualPrice).HasPrecision(18, 2);
		builder.HasIndex((CompanyPlan p) => p.Code).IsUnique();
	}
}
