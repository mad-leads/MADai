using MADai.Domain.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MADai.Infrastructure.Persistence.Configurations;

public class TaskRetryConfiguration : IEntityTypeConfiguration<TaskRetry>
{
	public void Configure(EntityTypeBuilder<TaskRetry> b)
	{
		b.ToTable("TaskRetries");
		b.Property((TaskRetry x) => x.FailureReason).HasMaxLength(2000);
		b.Property((TaskRetry x) => x.Strategy).HasMaxLength(40);
		b.HasIndex((TaskRetry x) => new { x.TaskId, x.AttemptNumber });
	}
}
