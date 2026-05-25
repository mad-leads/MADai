using MADai.Domain.Notifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MADai.Infrastructure.Persistence.Configurations;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
	public void Configure(EntityTypeBuilder<Notification> b)
	{
		b.ToTable("Notifications");
		b.Property((Notification x) => x.Channel).HasMaxLength(40);
		b.Property((Notification x) => x.Severity).HasMaxLength(20);
		b.Property((Notification x) => x.Title).IsRequired().HasMaxLength(200);
		b.Property((Notification x) => x.Body).HasMaxLength(2000);
		b.Property((Notification x) => x.Url).HasMaxLength(500);
		b.Property((Notification x) => x.Tags).HasMaxLength(500);
		b.HasIndex((Notification x) => new { x.UserId, x.CreatedAt });
	}
}
