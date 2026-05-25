using MADai.Domain.Workers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MADai.Infrastructure.Persistence.Configurations;

public class ArchitectureSummaryConfiguration : IEntityTypeConfiguration<ArchitectureSummary>
{
	public void Configure(EntityTypeBuilder<ArchitectureSummary> builder)
	{
		builder.ToTable("ArchitectureSummaries");
		builder.Property((ArchitectureSummary x) => x.RepositoryKey).IsRequired().HasMaxLength(160);
		builder.Property((ArchitectureSummary x) => x.Scope).IsRequired().HasMaxLength(80);
		builder.HasIndex((ArchitectureSummary x) => new { x.RepositoryKey, x.Scope });
	}
}
