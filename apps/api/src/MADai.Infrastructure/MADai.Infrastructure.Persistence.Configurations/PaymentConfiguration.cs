using MADai.Domain.Billing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MADai.Infrastructure.Persistence.Configurations;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
	public void Configure(EntityTypeBuilder<Payment> b)
	{
		b.ToTable("Payments");
		b.Property((Payment x) => x.Amount).HasPrecision(18, 2);
		b.Property((Payment x) => x.Currency).HasMaxLength(10);
		b.Property((Payment x) => x.Provider).HasMaxLength(40);
		b.Property((Payment x) => x.ProviderReference).HasMaxLength(200);
		b.HasOne((Payment x) => x.Invoice).WithMany().HasForeignKey((Payment x) => x.InvoiceId)
			.OnDelete(DeleteBehavior.NoAction);
	}
}
