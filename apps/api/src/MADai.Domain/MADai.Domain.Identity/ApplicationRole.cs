using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace MADai.Domain.Identity;

public class ApplicationRole : IdentityRole<Guid>
{
	public string? Description { get; set; }

	public bool IsSystem { get; set; }

	public ICollection<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();

}
