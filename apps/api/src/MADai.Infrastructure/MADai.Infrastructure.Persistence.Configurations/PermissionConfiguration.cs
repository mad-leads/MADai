using MADai.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MADai.Infrastructure.Persistence.Configurations;

public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
	public void Configure(EntityTypeBuilder<Permission> builder)
	{
		builder.ToTable("Permissions");
		builder.HasKey((Permission p) => p.Id);
		builder.Property((Permission p) => p.Code).IsRequired().HasMaxLength(120);
		builder.Property((Permission p) => p.DisplayName).IsRequired().HasMaxLength(200);
		builder.Property((Permission p) => p.Description).HasMaxLength(500);
		builder.Property((Permission p) => p.Category).HasMaxLength(80);
		builder.HasIndex((Permission p) => p.Code).IsUnique();
	}
}
