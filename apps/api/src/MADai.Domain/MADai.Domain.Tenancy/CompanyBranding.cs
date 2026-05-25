using System;
using MADai.Domain.Common;

namespace MADai.Domain.Tenancy;

public class CompanyBranding : Entity
{
	public Guid CompanyId { get; set; }

	public Company? Company { get; set; }

	public string? LogoUrl { get; set; }

	public string? FaviconUrl { get; set; }

	public string PrimaryColor { get; set; } = "#7c5cff";


	public string AccentColor { get; set; } = "#22d3ee";


	public string BackgroundColor { get; set; } = "#0b1020";


	public string? CustomCss { get; set; }

	public string ThemeMode { get; set; } = "dark";


	public string? EmailFromName { get; set; }

	public string? EmailFromAddress { get; set; }
}
