using MADai.Domain.Notifications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MADai.Infrastructure.Persistence.Configurations;

public class NotificationPreferenceConfiguration : IEntityTypeConfiguration<NotificationPreference>
{
	public void Configure(EntityTypeBuilder<NotificationPreference> b)
	{
		b.ToTable("NotificationPreferences");
		b.Property((NotificationPreference x) => x.Category).IsRequired().HasMaxLength(80);
		b.HasIndex((NotificationPreference x) => new { x.UserId, x.Category }).IsUnique();
	}
}
