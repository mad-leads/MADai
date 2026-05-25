using MADai.Domain.Workers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MADai.Infrastructure.Persistence.Configurations;

public class SessionMetricConfiguration : IEntityTypeConfiguration<SessionMetric>
{
	public void Configure(EntityTypeBuilder<SessionMetric> builder)
	{
		builder.ToTable("SessionMetrics");
		builder.Property((SessionMetric x) => x.SessionId).IsRequired().HasMaxLength(120);
		builder.Property((SessionMetric x) => x.RepositoryKey).IsRequired().HasMaxLength(160);
		builder.Property((SessionMetric x) => x.Health).IsRequired().HasMaxLength(80);
		builder.HasIndex((SessionMetric x) => new { x.RepositoryKey, x.SessionId, x.Timestamp });
	}
}
