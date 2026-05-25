using System;
using System.Collections.Generic;
using MADai.Domain.Tenancy;
using Microsoft.AspNetCore.Identity;

namespace MADai.Domain.Identity;

public class ApplicationUser : IdentityUser<Guid>
{
	public string? FirstName { get; set; }

	public string? LastName { get; set; }

	public string? AvatarUrl { get; set; }

	public string? TimeZone { get; set; }

	public string? Locale { get; set; }

	public bool IsActive { get; set; } = true;


	public bool IsMfaEnrolled { get; set; }

	public DateTime CreatedDate { get; set; } = DateTime.UtcNow;


	public DateTime? LastLoginAt { get; set; }

	public string? LastLoginIp { get; set; }

	public Guid? CompanyId { get; set; }

	public Company? Company { get; set; }

	public bool IsDeleted { get; set; }

	public DateTime? DeletedDate { get; set; }

	public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();


	public ICollection<LoginHistory> LoginHistory { get; set; } = new List<LoginHistory>();


	public ICollection<ApiKey> ApiKeys { get; set; } = new List<ApiKey>();


	public string DisplayName
	{
		get
		{
			if (!string.IsNullOrWhiteSpace(FirstName) || !string.IsNullOrWhiteSpace(LastName))
			{
				return (FirstName + " " + LastName).Trim();
			}
			return UserName ?? Email ?? Id.ToString();
		}
	}
}
