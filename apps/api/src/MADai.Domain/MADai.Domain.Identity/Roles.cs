using System.Collections.Generic;

namespace MADai.Domain.Identity;

public static class Roles
{
	public const string SystemAdmin = "SystemAdmin";

	public const string CompanyAdmin = "CompanyAdmin";

	public const string CompanyManager = "CompanyManager";

	public const string Worker = "Worker";

	public const string User = "User";

	public const string Client = "Client";

	public const string ReadOnly = "ReadOnly";

	public static readonly IReadOnlyList<string> All = new string[7] { "SystemAdmin", "CompanyAdmin", "CompanyManager", "Worker", "User", "Client", "ReadOnly" };
}
