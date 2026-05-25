using MADai.Domain.Workers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MADai.Infrastructure.Persistence.Configurations;

public class WorkerQueueConfiguration : IEntityTypeConfiguration<WorkerQueue>
{
	public void Configure(EntityTypeBuilder<WorkerQueue> builder)
	{
		builder.ToTable("WorkerQueues");
		builder.HasKey((WorkerQueue q) => q.Id);
		builder.Property((WorkerQueue q) => q.Name).IsRequired().HasMaxLength(80);
		builder.HasIndex((WorkerQueue q) => new { q.Name, q.CompanyId }).IsUnique();
	}
}
