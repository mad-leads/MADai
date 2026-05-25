using MADai.Domain.Billing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MADai.Infrastructure.Persistence.Configurations;

public class PlanConfiguration : IEntityTypeConfiguration<Plan>
{
	public void Configure(EntityTypeBuilder<Plan> b)
	{
		b.ToTable("Plans");
		b.Property((Plan x) => x.Name).IsRequired().HasMaxLength(120);
		b.Property((Plan x) => x.Code).IsRequired().HasMaxLength(40);
		b.Property((Plan x) => x.Currency).HasMaxLength(10);
		b.Property((Plan x) => x.MonthlyPrice).HasPrecision(18, 2);
		b.Property((Plan x) => x.AnnualPrice).HasPrecision(18, 2);
		b.HasIndex((Plan x) => x.Code).IsUnique();
	}
}
