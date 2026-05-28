using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MADai.Api.Controllers;

[ApiController]
[Route("api/products")]
[AllowAnonymous]
public sealed class ProductsController : ControllerBase
{
	[HttpGet("mad-universe")]
	public ActionResult<IReadOnlyList<MADProductResponse>> Universe()
	{
		return Ok(new[]
		{
			Product("MADProspects", "madprospects", "https://madprospects.com", "https://madprospectsapi.madprospects.com", "Prospecting, CRM, and universe entry point."),
			Product("MADCloud", "madcloud", "https://madcloud.madprospects.com", "https://madcloudapi.madprospects.com", "Sole AI integration and orchestration provider."),
			Product("MADai", "madai", "https://madai.madprospects.com", "https://madaiapi.madprospects.com", "AI command center and operator cockpit."),
			Product("MADAuthor", "madauthor", "https://madauthor.madprospects.com", "https://madauthorapi.madprospects.com", "Authoring workflows and publication support."),
			Product("MADCreate", "madcreate", "https://madcreate.madprospects.com", "https://madcreateapi.madprospects.com", "Creative asset and campaign production."),
			Product("MADHub", "madhub", "https://madhub.madprospects.com", "https://madhubapi.madprospects.com", "Shared operational hub."),
			Product("MADLeads", "madleads", "https://madleads.madprospects.com", "https://madleadsapi.madprospects.com", "Lead enrichment and activation."),
			Product("MADLearn", "madlearn", "https://madlearn.madprospects.com", "https://madlearnapi.madprospects.com", "Learning paths and knowledge operations."),
			Product("MADLove", "madlove", "https://madlove.madprospects.com", "https://madloveapi.madprospects.com", "Community and relationship workflows."),
			Product("MADMultisciple", "madmultisciple", "https://madmultisciple.madprospects.com", "https://madmultiscipleapi.madprospects.com", "Multi-discipline execution workflows."),
			Product("MADPulse", "madpulse", "https://madpulse.madprospects.com", "https://madpulseapi.madprospects.com", "Signals, monitoring, and performance pulse."),
			Product("MADRecruiting", "madrecruiting", "https://madrecruiting.madprospects.com", "https://madrecruitingapi.madprospects.com", "Recruiting workflows and candidate operations.")
		});
	}

	private static MADProductResponse Product(string name, string slug, string frontendUrl, string apiUrl, string purpose)
	{
		return new MADProductResponse(name, slug, frontendUrl, apiUrl, purpose, "/ai", "MADCloud", "Payfast");
	}

	public sealed record MADProductResponse(string Name, string Slug, string FrontendUrl, string ApiUrl, string Purpose, string AiRoute, string AiProvider, string PaymentProvider);
}
