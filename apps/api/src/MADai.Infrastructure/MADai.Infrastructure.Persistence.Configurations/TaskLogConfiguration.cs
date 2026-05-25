using MADai.Domain.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MADai.Infrastructure.Persistence.Configurations;

public class TaskLogConfiguration : IEntityTypeConfiguration<TaskLog>
{
	public void Configure(EntityTypeBuilder<TaskLog> b)
	{
		b.ToTable("TaskLogs");
		b.Property((TaskLog x) => x.Level).IsRequired().HasMaxLength(20);
		b.Property((TaskLog x) => x.Message).IsRequired().HasMaxLength(4000);
		b.Property((TaskLog x) => x.Source).HasMaxLength(120);
		b.HasIndex((TaskLog x) => new { x.TaskId, x.Timestamp });
	}
}
