using MADai.Domain.Workers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MADai.Infrastructure.Persistence.Configurations;

public class WorkerHeartbeatConfiguration : IEntityTypeConfiguration<WorkerHeartbeat>
{
	public void Configure(EntityTypeBuilder<WorkerHeartbeat> b)
	{
		b.ToTable("WorkerHeartbeats");
		b.Property((WorkerHeartbeat x) => x.Notes).HasMaxLength(500);
		b.HasIndex((WorkerHeartbeat x) => new { x.WorkerNodeId, x.Timestamp });
	}
}
