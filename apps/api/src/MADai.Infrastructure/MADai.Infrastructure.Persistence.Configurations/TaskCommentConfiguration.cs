using MADai.Domain.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MADai.Infrastructure.Persistence.Configurations;

public class TaskCommentConfiguration : IEntityTypeConfiguration<TaskComment>
{
	public void Configure(EntityTypeBuilder<TaskComment> b)
	{
		b.ToTable("TaskComments");
		b.Property((TaskComment x) => x.Body).IsRequired().HasMaxLength(8000);
	}
}
