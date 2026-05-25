using MADai.Domain.Workers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MADai.Infrastructure.Persistence.Configurations;

public class EntityMapConfiguration : IEntityTypeConfiguration<EntityMap>
{
	public void Configure(EntityTypeBuilder<EntityMap> builder)
	{
		builder.ToTable("EntityMaps");
		builder.Property((EntityMap x) => x.RepositoryKey).IsRequired().HasMaxLength(160);
		builder.HasIndex((EntityMap x) => x.RepositoryKey);
	}
}
