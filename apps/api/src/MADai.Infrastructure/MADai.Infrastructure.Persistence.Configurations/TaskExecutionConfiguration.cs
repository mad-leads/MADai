using MADai.Domain.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MADai.Infrastructure.Persistence.Configurations;

public class TaskExecutionConfiguration : IEntityTypeConfiguration<TaskExecution>
{
	public void Configure(EntityTypeBuilder<TaskExecution> b)
	{
		b.ToTable("TaskExecutions");
		b.Property((TaskExecution x) => x.WorkspacePath).HasMaxLength(500);
		b.Property((TaskExecution x) => x.OutputSummary).HasMaxLength(4000);
		b.Property((TaskExecution x) => x.ErrorMessage).HasMaxLength(2000);
		b.HasOne((TaskExecution x) => x.WorkerNode).WithMany().HasForeignKey((TaskExecution x) => x.WorkerNodeId)
			.OnDelete(DeleteBehavior.NoAction);
		b.HasIndex((TaskExecution x) => new { x.TaskId, x.AttemptNumber });
	}
}
