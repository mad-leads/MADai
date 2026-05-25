using MADai.Domain.Files;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MADai.Infrastructure.Persistence.Configurations;

public class FileFolderConfiguration : IEntityTypeConfiguration<FileFolder>
{
	public void Configure(EntityTypeBuilder<FileFolder> b)
	{
		b.ToTable("FileFolders");
		b.Property((FileFolder x) => x.Name).IsRequired().HasMaxLength(200);
		b.Property((FileFolder x) => x.Path).HasMaxLength(1000);
		b.HasOne((FileFolder x) => x.ParentFolder).WithMany((FileFolder x) => x.Children).HasForeignKey((FileFolder x) => x.ParentFolderId)
			.OnDelete(DeleteBehavior.NoAction);
	}
}
