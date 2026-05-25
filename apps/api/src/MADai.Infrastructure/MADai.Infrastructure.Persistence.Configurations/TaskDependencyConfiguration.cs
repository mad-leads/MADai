using MADai.Domain.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MADai.Infrastructure.Persistence.Configurations;

public class TaskDependencyConfiguration : IEntityTypeConfiguration<TaskDependency>
{
	public void Configure(EntityTypeBuilder<TaskDependency> builder)
	{
		builder.ToTable("TaskDependencies");
		builder.HasKey((TaskDependency d) => d.Id);
		builder.Property((TaskDependency d) => d.DependencyType).IsRequired().HasMaxLength(40);
		builder.HasOne((TaskDependency d) => d.DependsOnTask).WithMany((TaskItem t) => t.DependentTasks).HasForeignKey((TaskDependency d) => d.DependsOnTaskId)
			.OnDelete(DeleteBehavior.NoAction);
		builder.HasIndex((TaskDependency d) => new { d.TaskId, d.DependsOnTaskId }).IsUnique();
	}
}
