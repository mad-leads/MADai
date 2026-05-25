using MADai.Domain.Billing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MADai.Infrastructure.Persistence.Configurations;

public class SubscriptionConfiguration : IEntityTypeConfiguration<Subscription>
{
	public void Configure(EntityTypeBuilder<Subscription> b)
	{
		b.ToTable("Subscriptions");
		b.Property((Subscription x) => x.BillingCycle).HasMaxLength(20);
		b.Property((Subscription x) => x.ExternalReference).HasMaxLength(200);
		b.HasOne((Subscription x) => x.Plan).WithMany().HasForeignKey((Subscription x) => x.PlanId)
			.OnDelete(DeleteBehavior.NoAction);
	}
}
