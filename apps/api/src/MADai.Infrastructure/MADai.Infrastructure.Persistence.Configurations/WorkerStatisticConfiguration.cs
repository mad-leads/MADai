using MADai.Domain.Workers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MADai.Infrastructure.Persistence.Configurations;

public class WorkerStatisticConfiguration : IEntityTypeConfiguration<WorkerStatistic>
{
	public void Configure(EntityTypeBuilder<WorkerStatistic> builder)
	{
		builder.ToTable("WorkerStatistics");
		builder.Property((WorkerStatistic x) => x.RepositoryKey).IsRequired().HasMaxLength(160);
		builder.Property((WorkerStatistic x) => x.MetricName).IsRequired().HasMaxLength(120);
		builder.Property((WorkerStatistic x) => x.Unit).HasMaxLength(40);
		builder.HasIndex((WorkerStatistic x) => new { x.RepositoryKey, x.MetricName, x.Timestamp });
	}
}
