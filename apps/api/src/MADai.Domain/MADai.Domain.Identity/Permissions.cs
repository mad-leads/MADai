using System.Collections.Generic;

namespace MADai.Domain.Identity;

public static class Permissions
{
	public static class Tasks
	{
		public const string View = "tasks.view";

		public const string Create = "tasks.create";

		public const string Update = "tasks.update";

		public const string Delete = "tasks.delete";

		public const string Cancel = "tasks.cancel";

		public const string Retry = "tasks.retry";

		public const string Assign = "tasks.assign";
	}

	public static class Workers
	{
		public const string View = "workers.view";

		public const string Manage = "workers.manage";

		public const string Drain = "workers.drain";
	}

	public static class Companies
	{
		public const string View = "companies.view";

		public const string Manage = "companies.manage";
	}

	public static class Users
	{
		public const string View = "users.view";

		public const string Manage = "users.manage";
	}

	public static class Audit
	{
		public const string View = "audit.view";

		public const string Manage = "audit.manage";
	}

	public static class System
	{
		public const string Manage = "system.manage";

		public const string FeatureFlags = "system.feature-flags";

		public const string Settings = "system.settings";
	}

	public static IEnumerable<(string Code, string DisplayName, string Category)> All()
	{
		yield return (Code: "tasks.view", DisplayName: "View Tasks", Category: "Tasks");
		yield return (Code: "tasks.create", DisplayName: "Create Tasks", Category: "Tasks");
		yield return (Code: "tasks.update", DisplayName: "Update Tasks", Category: "Tasks");
		yield return (Code: "tasks.delete", DisplayName: "Delete Tasks", Category: "Tasks");
		yield return (Code: "tasks.cancel", DisplayName: "Cancel Tasks", Category: "Tasks");
		yield return (Code: "tasks.retry", DisplayName: "Retry Tasks", Category: "Tasks");
		yield return (Code: "tasks.assign", DisplayName: "Assign Tasks", Category: "Tasks");
		yield return (Code: "workers.view", DisplayName: "View Workers", Category: "Workers");
		yield return (Code: "workers.manage", DisplayName: "Manage Workers", Category: "Workers");
		yield return (Code: "workers.drain", DisplayName: "Drain Workers", Category: "Workers");
		yield return (Code: "companies.view", DisplayName: "View Companies", Category: "Companies");
		yield return (Code: "companies.manage", DisplayName: "Manage Companies", Category: "Companies");
		yield return (Code: "users.view", DisplayName: "View Users", Category: "Users");
		yield return (Code: "users.manage", DisplayName: "Manage Users", Category: "Users");
		yield return (Code: "audit.view", DisplayName: "View Audit Findings", Category: "Audit");
		yield return (Code: "audit.manage", DisplayName: "Manage Audit Findings", Category: "Audit");
		yield return (Code: "system.manage", DisplayName: "System Management", Category: "System");
		yield return (Code: "system.feature-flags", DisplayName: "Manage Feature Flags", Category: "System");
		yield return (Code: "system.settings", DisplayName: "Manage System Settings", Category: "System");
	}
}
