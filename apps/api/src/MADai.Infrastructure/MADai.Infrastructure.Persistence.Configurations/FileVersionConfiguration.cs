using MADai.Domain.Files;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MADai.Infrastructure.Persistence.Configurations;

public class FileVersionConfiguration : IEntityTypeConfiguration<FileVersion>
{
	public void Configure(EntityTypeBuilder<FileVersion> b)
	{
		b.ToTable("FileVersions");
		b.Property((FileVersion x) => x.StoragePath).IsRequired().HasMaxLength(500);
		b.Property((FileVersion x) => x.Checksum).HasMaxLength(128);
	}
}
