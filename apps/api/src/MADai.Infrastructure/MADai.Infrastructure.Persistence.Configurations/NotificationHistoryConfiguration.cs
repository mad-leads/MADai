using MADai.Domain.Notifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MADai.Infrastructure.Persistence.Configurations;

public class NotificationHistoryConfiguration : IEntityTypeConfiguration<NotificationHistory>
{
	public void Configure(EntityTypeBuilder<NotificationHistory> b)
	{
		b.ToTable("NotificationHistory");
		b.Property((NotificationHistory x) => x.TemplateCode).IsRequired().HasMaxLength(80);
		b.Property((NotificationHistory x) => x.Channel).HasMaxLength(40);
		b.Property((NotificationHistory x) => x.Recipient).IsRequired().HasMaxLength(254);
		b.Property((NotificationHistory x) => x.Status).HasMaxLength(40);
		b.Property((NotificationHistory x) => x.ErrorMessage).HasMaxLength(2000);
	}
}
