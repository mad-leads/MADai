using MADai.Domain.Workers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MADai.Infrastructure.Persistence.Configurations;

public class RepositoryCacheConfiguration : IEntityTypeConfiguration<RepositoryCache>
{
	public void Configure(EntityTypeBuilder<RepositoryCache> builder)
	{
		builder.ToTable("RepositoryCaches");
		builder.Property((RepositoryCache x) => x.RepositoryKey).IsRequired().HasMaxLength(160);
		builder.Property((RepositoryCache x) => x.CacheKey).IsRequired().HasMaxLength(200);
		builder.HasIndex((RepositoryCache x) => new { x.RepositoryKey, x.CacheKey }).IsUnique();
		builder.HasIndex((RepositoryCache x) => x.ExpiresAt);
	}
}
