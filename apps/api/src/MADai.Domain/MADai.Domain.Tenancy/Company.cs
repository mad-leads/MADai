using System;
using System.Collections.Generic;
using MADai.Domain.Common;
using MADai.Domain.Identity;

namespace MADai.Domain.Tenancy;

public class Company : AuditableEntity
{
	public string Name { get; set; } = string.Empty;


	public string Slug { get; set; } = string.Empty;


	public string? LegalName { get; set; }

	public string? ContactEmail { get; set; }

	public string? ContactPhone { get; set; }

	public string? Website { get; set; }

	public string? Country { get; set; }

	public string? Timezone { get; set; }

	public bool IsActive { get; set; } = true;


	public Guid? PlanId { get; set; }

	public CompanyBranding? Branding { get; set; }

	public CompanySettings? Settings { get; set; }

	public ICollection<ApplicationUser> Users { get; set; } = new List<ApplicationUser>();

}
