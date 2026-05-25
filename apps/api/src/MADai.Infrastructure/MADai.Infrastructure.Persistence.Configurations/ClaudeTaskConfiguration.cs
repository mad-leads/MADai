using MADai.Domain.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MADai.Infrastructure.Persistence.Configurations;

public class ClaudeTaskConfiguration : IEntityTypeConfiguration<ClaudeTask>
{
	public void Configure(EntityTypeBuilder<ClaudeTask> b)
	{
		b.ToTable("ClaudeTasks");
		b.HasKey((ClaudeTask t) => t.Id);
		b.Property((ClaudeTask t) => t.Title).IsRequired().HasMaxLength(300);
		b.Property((ClaudeTask t) => t.Description).HasMaxLength(8000);
		b.Property((ClaudeTask t) => t.Notes).HasMaxLength(8000);
		b.Property((ClaudeTask t) => t.AttachmentsJson).HasColumnType("nvarchar(max)");
		b.HasIndex((ClaudeTask t) => new { t.Status, t.Priority, t.Id });
		b.HasIndex((ClaudeTask t) => t.CreatedDate);
	}
}
