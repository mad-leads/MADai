using System.Collections.Generic;
using MADai.Domain.Common;

namespace MADai.Domain.Tasks;

public class TaskTag : TenantEntity
{
	public string Name { get; set; } = string.Empty;


	public string Color { get; set; } = "#7c5cff";


	public ICollection<TaskTagLink> TagLinks { get; set; } = new List<TaskTagLink>();

}
