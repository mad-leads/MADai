using MADai.Domain.Billing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MADai.Infrastructure.Persistence.Configurations;

public class UsageRecordConfiguration : IEntityTypeConfiguration<UsageRecord>
{
	public void Configure(EntityTypeBuilder<UsageRecord> b)
	{
		b.ToTable("UsageRecords");
		b.Property((UsageRecord x) => x.MetricName).IsRequired().HasMaxLength(80);
		b.Property((UsageRecord x) => x.Notes).HasMaxLength(500);
	}
}
