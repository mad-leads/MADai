using MADai.Domain.Billing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MADai.Infrastructure.Persistence.Configurations;

public class CreditLedgerConfiguration : IEntityTypeConfiguration<CreditLedger>
{
	public void Configure(EntityTypeBuilder<CreditLedger> b)
	{
		b.ToTable("CreditLedgers");
		b.Property((CreditLedger x) => x.Amount).HasPrecision(18, 2);
		b.Property((CreditLedger x) => x.Reason).IsRequired().HasMaxLength(200);
		b.Property((CreditLedger x) => x.Reference).HasMaxLength(200);
		b.Property((CreditLedger x) => x.Currency).HasMaxLength(10);
	}
}
