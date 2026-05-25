using MADai.Domain.Workers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MADai.Infrastructure.Persistence.Configurations;

public class WorkerMetricConfiguration : IEntityTypeConfiguration<WorkerMetric>
{
	public void Configure(EntityTypeBuilder<WorkerMetric> b)
	{
		b.ToTable("WorkerMetrics");
		b.Property((WorkerMetric x) => x.MetricName).IsRequired().HasMaxLength(80);
		b.Property((WorkerMetric x) => x.Unit).HasMaxLength(20);
		b.Property((WorkerMetric x) => x.Tags).HasMaxLength(500);
		b.HasIndex((WorkerMetric x) => new { x.WorkerNodeId, x.MetricName, x.Timestamp });
	}
}
