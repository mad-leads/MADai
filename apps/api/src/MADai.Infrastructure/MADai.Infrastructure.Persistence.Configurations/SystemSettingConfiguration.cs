using MADai.Domain.SystemEntities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MADai.Infrastructure.Persistence.Configurations;

public class SystemSettingConfiguration : IEntityTypeConfiguration<SystemSetting>
{
	public void Configure(EntityTypeBuilder<SystemSetting> b)
	{
		b.ToTable("SystemSettings");
		b.Property((SystemSetting x) => x.Key).IsRequired().HasMaxLength(120);
		b.Property((SystemSetting x) => x.Category).HasMaxLength(80);
		b.Property((SystemSetting x) => x.DataType).HasMaxLength(40);
		b.Property((SystemSetting x) => x.Description).HasMaxLength(1000);
		b.HasIndex((SystemSetting x) => x.Key).IsUnique();
	}
}
