using MADai.Domain.Workers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MADai.Infrastructure.Persistence.Configurations;

public class WorkerProcessConfiguration : IEntityTypeConfiguration<WorkerProcess>
{
	public void Configure(EntityTypeBuilder<WorkerProcess> builder)
	{
		builder.ToTable("WorkerProcesses");
		builder.Property((WorkerProcess x) => x.RepositoryKey).IsRequired().HasMaxLength(160);
		builder.Property((WorkerProcess x) => x.ProcessKey).IsRequired().HasMaxLength(160);
		builder.Property((WorkerProcess x) => x.SessionId).IsRequired().HasMaxLength(120);
		builder.Property((WorkerProcess x) => x.Status).IsRequired().HasMaxLength(80);
		builder.HasIndex((WorkerProcess x) => new { x.RepositoryKey, x.ProcessKey });
		builder.HasIndex((WorkerProcess x) => x.SessionId);
	}
}
