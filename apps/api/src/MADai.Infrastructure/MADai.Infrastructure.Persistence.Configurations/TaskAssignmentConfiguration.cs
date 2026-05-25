using MADai.Domain.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MADai.Infrastructure.Persistence.Configurations;

public class TaskAssignmentConfiguration : IEntityTypeConfiguration<TaskAssignment>
{
	public void Configure(EntityTypeBuilder<TaskAssignment> b)
	{
		b.ToTable("TaskAssignments");
		b.Property((TaskAssignment x) => x.AssignmentType).HasMaxLength(20);
		b.Property((TaskAssignment x) => x.Notes).HasMaxLength(1000);
		b.HasOne((TaskAssignment x) => x.AssignedWorker).WithMany().HasForeignKey((TaskAssignment x) => x.AssignedWorkerId)
			.OnDelete(DeleteBehavior.NoAction);
		b.HasIndex((TaskAssignment x) => x.TaskId);
	}
}
