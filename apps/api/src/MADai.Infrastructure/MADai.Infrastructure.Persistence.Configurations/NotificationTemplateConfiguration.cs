using MADai.Domain.Notifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MADai.Infrastructure.Persistence.Configurations;

public class NotificationTemplateConfiguration : IEntityTypeConfiguration<NotificationTemplate>
{
	public void Configure(EntityTypeBuilder<NotificationTemplate> b)
	{
		b.ToTable("NotificationTemplates");
		b.Property((NotificationTemplate x) => x.Code).IsRequired().HasMaxLength(80);
		b.Property((NotificationTemplate x) => x.Name).IsRequired().HasMaxLength(200);
		b.Property((NotificationTemplate x) => x.Channel).HasMaxLength(40);
		b.Property((NotificationTemplate x) => x.Subject).IsRequired().HasMaxLength(200);
		b.Property((NotificationTemplate x) => x.Body).IsRequired();
		b.HasIndex((NotificationTemplate x) => x.Code).IsUnique();
	}
}
