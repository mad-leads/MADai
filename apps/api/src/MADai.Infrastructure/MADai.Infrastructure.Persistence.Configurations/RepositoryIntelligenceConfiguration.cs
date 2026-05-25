using MADai.Domain.Workers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MADai.Infrastructure.Persistence.Configurations;

public class RepositoryIntelligenceConfiguration : IEntityTypeConfiguration<RepositoryIntelligence>
{
	public void Configure(EntityTypeBuilder<RepositoryIntelligence> builder)
	{
		builder.ToTable("RepositoryIntelligence");
		builder.Property((RepositoryIntelligence x) => x.RepositoryKey).IsRequired().HasMaxLength(160);
		builder.Property((RepositoryIntelligence x) => x.RepositoryPath).IsRequired().HasMaxLength(600);
		builder.Property((RepositoryIntelligence x) => x.BranchName).HasMaxLength(160);
		builder.Property((RepositoryIntelligence x) => x.CommitSha).HasMaxLength(80);
		builder.Property((RepositoryIntelligence x) => x.CacheFingerprint).HasMaxLength(120);
		builder.HasIndex((RepositoryIntelligence x) => x.RepositoryKey).IsUnique();
		builder.HasIndex((RepositoryIntelligence x) => x.ScannedAt);
	}
}
