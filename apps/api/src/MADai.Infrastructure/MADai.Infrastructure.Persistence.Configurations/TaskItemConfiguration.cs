using MADai.Domain.Tasks;
using MADai.Domain.Workers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MADai.Infrastructure.Persistence.Configurations;

public class TaskItemConfiguration : IEntityTypeConfiguration<TaskItem>
{
	public void Configure(EntityTypeBuilder<TaskItem> builder)
	{
		builder.ToTable("Tasks");
		builder.HasKey((TaskItem t) => t.Id);
		builder.Property((TaskItem t) => t.Title).IsRequired().HasMaxLength(200);
		builder.Property((TaskItem t) => t.Description).HasMaxLength(8000);
		builder.Property((TaskItem t) => t.QueueName).HasMaxLength(80);
		builder.Property((TaskItem t) => t.PromptPayload);
		builder.Property((TaskItem t) => t.InputPayload);
		builder.Property((TaskItem t) => t.ResultPayload);
		builder.Property((TaskItem t) => t.OutputSummary).HasMaxLength(4000);
		builder.Property((TaskItem t) => t.ErrorMessage).HasMaxLength(2000);
		builder.Property((TaskItem t) => t.WorkspacePath).HasMaxLength(500);
		builder.Property((TaskItem t) => t.ClaimToken).HasMaxLength(64);
		builder.Property((TaskItem t) => t.Tags).HasMaxLength(2000);
		builder.Property((TaskItem t) => t.CronExpression).HasMaxLength(100);
		builder.Property((TaskItem t) => t.ValidationReport);
		builder.HasIndex((TaskItem t) => t.Status);
		builder.HasIndex((TaskItem t) => t.Priority);
		builder.HasIndex((TaskItem t) => new { t.CompanyId, t.Status });
		builder.HasIndex((TaskItem t) => new { t.QueueName, t.Status });
		builder.HasIndex((TaskItem t) => t.CreatedDate);
		builder.HasOne((TaskItem t) => t.ParentTask).WithMany((TaskItem t) => t.Subtasks).HasForeignKey((TaskItem t) => t.ParentTaskId)
			.OnDelete(DeleteBehavior.NoAction);
		builder.HasOne((TaskItem t) => t.ClaimedByWorker).WithMany((WorkerNode w) => w.ClaimedTasks).HasForeignKey((TaskItem t) => t.ClaimedByWorkerId)
			.OnDelete(DeleteBehavior.SetNull);
		builder.HasOne((TaskItem t) => t.Template).WithMany().HasForeignKey((TaskItem t) => t.TemplateId)
			.OnDelete(DeleteBehavior.SetNull);
		builder.HasMany((TaskItem t) => t.Dependencies).WithOne((TaskDependency d) => d.Task).HasForeignKey((TaskDependency d) => d.TaskId)
			.OnDelete(DeleteBehavior.Cascade);
		builder.HasMany((TaskItem t) => t.Comments).WithOne((TaskComment c) => c.Task).HasForeignKey((TaskComment c) => c.TaskId)
			.OnDelete(DeleteBehavior.Cascade);
		builder.HasMany((TaskItem t) => t.Logs).WithOne((TaskLog l) => l.Task).HasForeignKey((TaskLog l) => l.TaskId)
			.OnDelete(DeleteBehavior.Cascade);
		builder.HasMany((TaskItem t) => t.Artifacts).WithOne((TaskArtifact a) => a.Task).HasForeignKey((TaskArtifact a) => a.TaskId)
			.OnDelete(DeleteBehavior.Cascade);
		builder.HasMany((TaskItem t) => t.Executions).WithOne((TaskExecution e) => e.Task).HasForeignKey((TaskExecution e) => e.TaskId)
			.OnDelete(DeleteBehavior.Cascade);
		builder.HasMany((TaskItem t) => t.TagLinks).WithOne((TaskTagLink l) => l.Task).HasForeignKey((TaskTagLink l) => l.TaskId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}
