using MADai.Domain.Webhooks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MADai.Infrastructure.Persistence.Configurations;

public class WebhookEventConfiguration : IEntityTypeConfiguration<WebhookEvent>
{
	public void Configure(EntityTypeBuilder<WebhookEvent> b)
	{
		b.ToTable("WebhookEvents");
		b.HasKey((WebhookEvent e) => e.Id);
		b.Property((WebhookEvent e) => e.EventType).IsRequired().HasMaxLength(80);
		b.Property((WebhookEvent e) => e.PayloadJson).IsRequired().HasColumnType("nvarchar(max)");
		b.Property((WebhookEvent e) => e.LastError).HasMaxLength(2000);
		b.Property((WebhookEvent e) => e.Status).IsRequired().HasMaxLength(20);
		b.HasIndex((WebhookEvent e) => new { e.Status, e.NextAttemptAt });
		b.HasIndex((WebhookEvent e) => new { e.EndpointId, e.CreatedAt });
	}
}
