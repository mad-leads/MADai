using System;
using Asp.Versioning;
using MADai.Shared.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MADai.Api.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/v{version:apiVersion}/system")]
[ApiVersion("1.0")]
public class SystemController : ControllerBase
{
	[HttpGet("ping")]
	public ActionResult<ApiResult<object>> Ping()
	{
		return Ok(ApiResult<object>.Ok(new
		{
			service = "MADai API",
			version = (typeof(SystemController).Assembly.GetName().Version?.ToString() ?? "1.0.0"),
			timeUtc = DateTime.UtcNow
		}));
	}
}
