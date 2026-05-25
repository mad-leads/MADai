using MADai.Domain.Files;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MADai.Infrastructure.Persistence.Configurations;

public class FileItemConfiguration : IEntityTypeConfiguration<FileItem>
{
	public void Configure(EntityTypeBuilder<FileItem> b)
	{
		b.ToTable("Files");
		b.Property((FileItem x) => x.Name).IsRequired().HasMaxLength(255);
		b.Property((FileItem x) => x.ContentType).HasMaxLength(120);
		b.Property((FileItem x) => x.StoragePath).IsRequired().HasMaxLength(500);
		b.Property((FileItem x) => x.StorageProvider).HasMaxLength(40);
		b.Property((FileItem x) => x.Checksum).HasMaxLength(128);
		b.Property((FileItem x) => x.ThumbnailPath).HasMaxLength(500);
		b.Property((FileItem x) => x.PreviewPath).HasMaxLength(500);
		b.HasOne((FileItem x) => x.Folder).WithMany((FileFolder f) => f.Files).HasForeignKey((FileItem x) => x.FolderId)
			.OnDelete(DeleteBehavior.NoAction);
		b.HasMany((FileItem x) => x.Versions).WithOne((FileVersion v) => v.File).HasForeignKey((FileVersion v) => v.FileId)
			.OnDelete(DeleteBehavior.Cascade);
		b.HasMany((FileItem x) => x.AccessLogs).WithOne((FileAccessLog l) => l.File).HasForeignKey((FileAccessLog l) => l.FileId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}
