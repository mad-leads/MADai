using MADai.Domain.SystemEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MADai.Infrastructure.Persistence.Configurations;

public class FeatureFlagConfiguration : IEntityTypeConfiguration<FeatureFlag>
{
	public void Configure(EntityTypeBuilder<FeatureFlag> b)
	{
		b.ToTable("FeatureFlags");
		b.Property((FeatureFlag x) => x.Key).IsRequired().HasMaxLength(120);
		b.Property((FeatureFlag x) => x.Name).IsRequired().HasMaxLength(200);
		b.Property((FeatureFlag x) => x.Description).HasMaxLength(1000);
		b.Property((FeatureFlag x) => x.Audience).HasMaxLength(500);
		b.HasIndex((FeatureFlag x) => x.Key).IsUnique();
	}
}
