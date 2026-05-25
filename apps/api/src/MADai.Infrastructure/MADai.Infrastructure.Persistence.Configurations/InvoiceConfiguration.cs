using MADai.Domain.Billing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MADai.Infrastructure.Persistence.Configurations;

public class InvoiceConfiguration : IEntityTypeConfiguration<Invoice>
{
	public void Configure(EntityTypeBuilder<Invoice> b)
	{
		b.ToTable("Invoices");
		b.Property((Invoice x) => x.Number).IsRequired().HasMaxLength(40);
		b.Property((Invoice x) => x.Subtotal).HasPrecision(18, 2);
		b.Property((Invoice x) => x.Tax).HasPrecision(18, 2);
		b.Property((Invoice x) => x.Total).HasPrecision(18, 2);
		b.Property((Invoice x) => x.Currency).HasMaxLength(10);
		b.HasIndex((Invoice x) => x.Number).IsUnique();
		b.HasOne((Invoice x) => x.Subscription).WithMany().HasForeignKey((Invoice x) => x.SubscriptionId)
			.OnDelete(DeleteBehavior.NoAction);
	}
}
