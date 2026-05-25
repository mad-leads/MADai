using MADai.Domain.Files;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MADai.Infrastructure.Persistence.Configurations;

public class FileAccessLogConfiguration : IEntityTypeConfiguration<FileAccessLog>
{
	public void Configure(EntityTypeBuilder<FileAccessLog> b)
	{
		b.ToTable("FileAccessLogs");
		b.Property((FileAccessLog x) => x.Action).HasMaxLength(40);
		b.Property((FileAccessLog x) => x.IpAddress).HasMaxLength(64);
	}
}
