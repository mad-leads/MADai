using MADai.Domain.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MADai.Infrastructure.Persistence.Configurations;

public class TaskTemplateConfiguration : IEntityTypeConfiguration<TaskTemplate>
{
	public void Configure(EntityTypeBuilder<TaskTemplate> b)
	{
		b.ToTable("TaskTemplates");
		b.Property((TaskTemplate x) => x.Name).IsRequired().HasMaxLength(120);
		b.Property((TaskTemplate x) => x.Description).HasMaxLength(2000);
		b.Property((TaskTemplate x) => x.QueueName).HasMaxLength(80);
		b.Property((TaskTemplate x) => x.RequiredCapabilities).HasMaxLength(500);
		b.HasIndex((TaskTemplate x) => new { x.CompanyId, x.Name });
	}
}
