using MADai.Domain.Workers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MADai.Infrastructure.Persistence.Configurations;

public class WorkerNodeConfiguration : IEntityTypeConfiguration<WorkerNode>
{
	public void Configure(EntityTypeBuilder<WorkerNode> builder)
	{
		builder.ToTable("WorkerNodes");
		builder.HasKey((WorkerNode w) => w.Id);
		builder.Property((WorkerNode w) => w.Name).IsRequired().HasMaxLength(120);
		builder.Property((WorkerNode w) => w.MachineName).IsRequired().HasMaxLength(200);
		builder.Property((WorkerNode w) => w.AgentVersion).HasMaxLength(40);
		builder.Property((WorkerNode w) => w.OperatingSystem).HasMaxLength(200);
		builder.Property((WorkerNode w) => w.IpAddress).HasMaxLength(64);
		builder.Property((WorkerNode w) => w.QueueName).HasMaxLength(80);
		builder.Property((WorkerNode w) => w.Capabilities).HasMaxLength(2000);
		builder.Property((WorkerNode w) => w.Labels).HasMaxLength(2000);
		builder.Property((WorkerNode w) => w.WorkspaceRoot).HasMaxLength(500);
		builder.Property((WorkerNode w) => w.ApiKeyHash).IsRequired().HasMaxLength(200);
		builder.HasIndex((WorkerNode w) => new { w.MachineName, w.Name, w.CompanyId });
		builder.HasIndex((WorkerNode w) => w.Status);
		builder.HasMany((WorkerNode w) => w.Heartbeats).WithOne((WorkerHeartbeat h) => h.WorkerNode).HasForeignKey((WorkerHeartbeat h) => h.WorkerNodeId)
			.OnDelete(DeleteBehavior.Cascade);
		builder.HasMany((WorkerNode w) => w.Metrics).WithOne((WorkerMetric m) => m.WorkerNode).HasForeignKey((WorkerMetric m) => m.WorkerNodeId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}
