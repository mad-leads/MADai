using MADai.Domain.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MADai.Infrastructure.Persistence.Configurations;

public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
	public void Configure(EntityTypeBuilder<RolePermission> builder)
	{
		builder.ToTable("RolePermissions");
		builder.HasKey((RolePermission rp) => rp.Id);
		builder.HasOne((RolePermission rp) => rp.Role).WithMany((ApplicationRole r) => r.RolePermissions).HasForeignKey((RolePermission rp) => rp.RoleId)
			.OnDelete(DeleteBehavior.Cascade);
		builder.HasOne((RolePermission rp) => rp.Permission).WithMany().HasForeignKey((RolePermission rp) => rp.PermissionId)
			.OnDelete(DeleteBehavior.Cascade);
		builder.HasIndex((RolePermission rp) => new { rp.RoleId, rp.PermissionId }).IsUnique();
	}
}
