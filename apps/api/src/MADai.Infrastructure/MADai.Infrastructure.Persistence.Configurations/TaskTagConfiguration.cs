using MADai.Domain.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MADai.Infrastructure.Persistence.Configurations;

public class TaskTagConfiguration : IEntityTypeConfiguration<TaskTag>
{
	public void Configure(EntityTypeBuilder<TaskTag> builder)
	{
		builder.ToTable("TaskTags");
		builder.HasKey((TaskTag t) => t.Id);
		builder.Property((TaskTag t) => t.Name).IsRequired().HasMaxLength(80);
		builder.Property((TaskTag t) => t.Color).HasMaxLength(20);
		builder.HasIndex((TaskTag t) => new { t.CompanyId, t.Name }).IsUnique();
	}
}
