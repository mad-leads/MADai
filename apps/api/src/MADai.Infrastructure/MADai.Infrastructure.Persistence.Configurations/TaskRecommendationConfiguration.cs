using MADai.Domain.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MADai.Infrastructure.Persistence.Configurations;

public class TaskRecommendationConfiguration : IEntityTypeConfiguration<TaskRecommendation>
{
	public void Configure(EntityTypeBuilder<TaskRecommendation> b)
	{
		b.ToTable("TaskRecommendations");
		b.Property((TaskRecommendation x) => x.Title).IsRequired().HasMaxLength(200);
		b.Property((TaskRecommendation x) => x.Body).IsRequired().HasMaxLength(4000);
		b.Property((TaskRecommendation x) => x.Source).HasMaxLength(40);
		b.Property((TaskRecommendation x) => x.Status).HasMaxLength(40);
		b.HasOne((TaskRecommendation x) => x.Task).WithMany().HasForeignKey((TaskRecommendation x) => x.TaskId)
			.OnDelete(DeleteBehavior.NoAction);
	}
}
