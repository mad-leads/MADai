using MADai.Domain.Workers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MADai.Infrastructure.Persistence.Configurations;

public class WorkerCapabilityConfiguration : IEntityTypeConfiguration<WorkerCapabilityEntry>
{
	public void Configure(EntityTypeBuilder<WorkerCapabilityEntry> builder)
	{
		builder.ToTable("WorkerCapabilities");
		builder.HasKey((WorkerCapabilityEntry c) => c.Id);
		builder.HasOne((WorkerCapabilityEntry c) => c.WorkerNode).WithMany().HasForeignKey((WorkerCapabilityEntry c) => c.WorkerNodeId)
			.OnDelete(DeleteBehavior.Cascade);
		builder.HasIndex((WorkerCapabilityEntry c) => new { c.WorkerNodeId, c.Capability }).IsUnique();
	}
}
