using MADai.Domain.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MADai.Infrastructure.Persistence.Configurations;

public class ClaudeTaskConfiguration : IEntityTypeConfiguration<ClaudeTask>
{
    public void Configure(EntityTypeBuilder<ClaudeTask> b)
    {
        b.ToTable("ClaudeTasks");
        b.HasKey(t => t.Id);
        b.Property(t => t.Title).IsRequired().HasMaxLength(300);
        b.Property(t => t.Description).HasMaxLength(8000);
        b.Property(t => t.Notes).HasMaxLength(8000);
        b.Property(t => t.AttachmentsJson).HasColumnType("nvarchar(max)");
        b.HasIndex(t => new { t.Status, t.Priority, t.Id });
        b.HasIndex(t => t.CreatedDate);
    }
}

public class ClaudePromptTemplateConfiguration : IEntityTypeConfiguration<ClaudePromptTemplate>
{
    public void Configure(EntityTypeBuilder<ClaudePromptTemplate> b)
    {
        b.ToTable("ClaudePromptTemplates");
        b.HasKey(t => t.Id);
        b.Property(t => t.Name).IsRequired().HasMaxLength(200);
        b.Property(t => t.Description).HasMaxLength(2000);
        b.Property(t => t.Content).IsRequired().HasColumnType("nvarchar(max)");
        b.HasIndex(t => t.Name).IsUnique();
    }
}
