using MADai.Domain.Workers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MADai.Infrastructure.Persistence.Configurations;

public class SessionCheckpointConfiguration : IEntityTypeConfiguration<SessionCheckpoint>
{
	public void Configure(EntityTypeBuilder<SessionCheckpoint> builder)
	{
		builder.ToTable("SessionCheckpoints");
		builder.Property((SessionCheckpoint x) => x.RepositoryKey).IsRequired().HasMaxLength(160);
		builder.Property((SessionCheckpoint x) => x.SessionId).IsRequired().HasMaxLength(120);
		builder.HasIndex((SessionCheckpoint x) => new { x.RepositoryKey, x.SessionId });
		builder.HasIndex((SessionCheckpoint x) => x.CapturedAt);
	}
}
