using MADai.Domain.Identity;
using MADai.Domain.Tenancy;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MADai.Infrastructure.Persistence.Configurations;

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
	public void Configure(EntityTypeBuilder<ApplicationUser> builder)
	{
		builder.Property((ApplicationUser u) => u.FirstName).HasMaxLength(80);
		builder.Property((ApplicationUser u) => u.LastName).HasMaxLength(80);
		builder.Property((ApplicationUser u) => u.AvatarUrl).HasMaxLength(500);
		builder.Property((ApplicationUser u) => u.TimeZone).HasMaxLength(80);
		builder.Property((ApplicationUser u) => u.Locale).HasMaxLength(20);
		builder.Property((ApplicationUser u) => u.LastLoginIp).HasMaxLength(64);
		builder.HasOne((ApplicationUser u) => u.Company).WithMany((Company c) => c.Users).HasForeignKey((ApplicationUser u) => u.CompanyId)
			.OnDelete(DeleteBehavior.NoAction);
		builder.HasIndex((ApplicationUser u) => u.CompanyId);
	}
}
