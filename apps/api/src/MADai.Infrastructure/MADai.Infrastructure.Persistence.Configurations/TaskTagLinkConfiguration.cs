using MADai.Domain.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MADai.Infrastructure.Persistence.Configurations;

public class TaskTagLinkConfiguration : IEntityTypeConfiguration<TaskTagLink>
{
	public void Configure(EntityTypeBuilder<TaskTagLink> builder)
	{
		builder.ToTable("TaskTagLinks");
		builder.HasKey((TaskTagLink l) => l.Id);
		builder.HasOne((TaskTagLink l) => l.Tag).WithMany((TaskTag t) => t.TagLinks).HasForeignKey((TaskTagLink l) => l.TagId)
			.OnDelete(DeleteBehavior.Cascade);
		builder.HasIndex((TaskTagLink l) => new { l.TaskId, l.TagId }).IsUnique();
	}
}
