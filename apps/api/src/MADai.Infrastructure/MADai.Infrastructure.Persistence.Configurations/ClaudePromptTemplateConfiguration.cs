using MADai.Domain.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MADai.Infrastructure.Persistence.Configurations;

public class ClaudePromptTemplateConfiguration : IEntityTypeConfiguration<ClaudePromptTemplate>
{
	public void Configure(EntityTypeBuilder<ClaudePromptTemplate> b)
	{
		b.ToTable("ClaudePromptTemplates");
		b.HasKey((ClaudePromptTemplate t) => t.Id);
		b.Property((ClaudePromptTemplate t) => t.Name).IsRequired().HasMaxLength(200);
		b.Property((ClaudePromptTemplate t) => t.Description).HasMaxLength(2000);
		b.Property((ClaudePromptTemplate t) => t.Content).IsRequired().HasColumnType("nvarchar(max)");
		b.HasIndex((ClaudePromptTemplate t) => t.Name).IsUnique();
	}
}
