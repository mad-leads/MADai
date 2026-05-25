using MADai.Domain.Workers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MADai.Infrastructure.Persistence.Configurations;

public class RouteMapConfiguration : IEntityTypeConfiguration<RouteMap>
{
	public void Configure(EntityTypeBuilder<RouteMap> builder)
	{
		builder.ToTable("RouteMaps");
		builder.Property((RouteMap x) => x.RepositoryKey).IsRequired().HasMaxLength(160);
		builder.Property((RouteMap x) => x.Kind).IsRequired().HasMaxLength(80);
		builder.HasIndex((RouteMap x) => new { x.RepositoryKey, x.Kind });
	}
}
