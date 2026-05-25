using MADai.Domain.Audit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MADai.Infrastructure.Persistence.Configurations;

public class CleanupTaskConfiguration : IEntityTypeConfiguration<CleanupTask>
{
	public void Configure(EntityTypeBuilder<CleanupTask> b)
	{
		b.ToTable("CleanupTasks");
		b.Property((CleanupTask x) => x.Target).IsRequired().HasMaxLength(200);
		b.Property((CleanupTask x) => x.Result).HasMaxLength(2000);
	}
}
