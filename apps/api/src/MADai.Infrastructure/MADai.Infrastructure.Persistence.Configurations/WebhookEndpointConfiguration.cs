using MADai.Domain.SystemEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MADai.Infrastructure.Persistence.Configurations;

public class WebhookEndpointConfiguration : IEntityTypeConfiguration<WebhookEndpoint>
{
	public void Configure(EntityTypeBuilder<WebhookEndpoint> b)
	{
		b.ToTable("WebhookEndpoints");
		b.Property((WebhookEndpoint x) => x.Url).IsRequired().HasMaxLength(500);
		b.Property((WebhookEndpoint x) => x.Secret).IsRequired().HasMaxLength(128);
		b.Property((WebhookEndpoint x) => x.EventsCsv).HasMaxLength(1000);
	}
}
