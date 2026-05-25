using System;
using MADai.Domain.Common;

namespace MADai.Domain.Identity;

public class RolePermission : Entity
{
	public Guid RoleId { get; set; }

	public ApplicationRole? Role { get; set; }

	public Guid PermissionId { get; set; }

	public Permission? Permission { get; set; }
}
