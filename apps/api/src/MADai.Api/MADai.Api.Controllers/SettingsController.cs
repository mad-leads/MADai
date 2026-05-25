using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Asp.Versioning;
using MADai.Application.Features.Settings;
using MADai.Shared.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MADai.Api.Controllers;

[ApiController]
[Authorize(Roles = "SystemAdmin")]
[Route("api/v{version:apiVersion}/settings")]
[ApiVersion("1.0")]
public class SettingsController : ControllerBase
{
	private readonly ISettingsService _settings;

	public SettingsController(ISettingsService settings)
	{
		_settings = settings;
	}

	[HttpGet]
	public async Task<ActionResult<ApiResult<IReadOnlyList<SystemSettingDto>>>> List([FromQuery] string? category, CancellationToken cancellationToken)
	{
		return Ok(ApiResult<IReadOnlyList<SystemSettingDto>>.Ok(await _settings.ListAsync(category, cancellationToken)));
	}

	[HttpPatch]
	public async Task<ActionResult<ApiResult<IReadOnlyDictionary<string, string?>>>> UpdateBatch([FromBody] Dictionary<string, string?> updates, CancellationToken cancellationToken)
	{
		if (updates == null || updates.Count == 0)
		{
			return BadRequest(ApiResult<IReadOnlyDictionary<string, string>>.Fail("At least one key/value is required."));
		}
		return Ok(ApiResult<IReadOnlyDictionary<string, string>>.Ok(await _settings.UpdateBatchAsync(updates, cancellationToken)));
	}
}
