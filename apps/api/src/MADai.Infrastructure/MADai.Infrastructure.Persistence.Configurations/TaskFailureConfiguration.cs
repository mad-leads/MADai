using MADai.Domain.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MADai.Infrastructure.Persistence.Configurations;

public class TaskFailureConfiguration : IEntityTypeConfiguration<TaskFailure>
{
	public void Configure(EntityTypeBuilder<TaskFailure> b)
	{
		b.ToTable("TaskFailures");
		b.Property((TaskFailure x) => x.Reason).IsRequired().HasMaxLength(2000);
		b.Property((TaskFailure x) => x.Category).HasMaxLength(40);
		b.HasIndex((TaskFailure x) => new { x.TaskId, x.FailedAt });
	}
}
