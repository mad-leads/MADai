using MADai.Domain.Workers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MADai.Infrastructure.Persistence.Configurations;

public class DependencyGraphConfiguration : IEntityTypeConfiguration<DependencyGraph>
{
	public void Configure(EntityTypeBuilder<DependencyGraph> builder)
	{
		builder.ToTable("DependencyGraphs");
		builder.Property((DependencyGraph x) => x.RepositoryKey).IsRequired().HasMaxLength(160);
		builder.HasIndex((DependencyGraph x) => x.RepositoryKey);
	}
}
