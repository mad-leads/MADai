using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Asp.Versioning;
using MADai.Application.Features.Admin;
using MADai.Domain.Identity;
using MADai.Shared.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MADai.Api.Controllers;

[ApiController]
[Authorize(Roles = "SystemAdmin")]
[Route("api/v{version:apiVersion}/admin")]
[ApiVersion("1.0")]
public class AdminController : ControllerBase
{
	private readonly IAdminUserService _users;

	private readonly IFeatureFlagAdminService _flags;

	private readonly IPlanAdminService _plans;

	private readonly ICompanyAdminService _companies;

	public AdminController(IAdminUserService users, IFeatureFlagAdminService flags, IPlanAdminService plans, ICompanyAdminService companies)
	{
		_users = users;
		_flags = flags;
		_plans = plans;
		_companies = companies;
	}

	[HttpGet("users")]
	public async Task<ActionResult<ApiResult<PagedResult<AdminUserDto>>>> ListUsers([FromQuery] PageQuery query, CancellationToken cancellationToken)
	{
		return Ok(ApiResult<PagedResult<AdminUserDto>>.Ok(await _users.ListAsync(query, cancellationToken)));
	}

	[HttpGet("users/{id:guid}")]
	public async Task<ActionResult<ApiResult<AdminUserDto>>> GetUser(Guid id, CancellationToken cancellationToken)
	{
		return Ok(ApiResult<AdminUserDto>.Ok(await _users.GetAsync(id, cancellationToken)));
	}

	[HttpPost("users")]
	public async Task<ActionResult<ApiResult<AdminUserDto>>> CreateUser([FromBody] CreateAdminUserRequest request, CancellationToken cancellationToken)
	{
		return Ok(ApiResult<AdminUserDto>.Ok(await _users.CreateAsync(request, cancellationToken)));
	}

	[HttpPatch("users/{id:guid}")]
	public async Task<ActionResult<ApiResult<AdminUserDto>>> UpdateUser(Guid id, [FromBody] UpdateAdminUserRequest request, CancellationToken cancellationToken)
	{
		return Ok(ApiResult<AdminUserDto>.Ok(await _users.UpdateAsync(id, request, cancellationToken)));
	}

	[HttpPost("users/{id:guid}/reset-password")]
	public async Task<ActionResult<ApiResult>> ResetUserPassword(Guid id, [FromBody] AdminResetPasswordRequest request, CancellationToken cancellationToken)
	{
		await _users.SetPasswordAsync(id, request.NewPassword, cancellationToken);
		return Ok(ApiResult.Ok());
	}

	[HttpDelete("users/{id:guid}")]
	public async Task<ActionResult<ApiResult>> DeleteUser(Guid id, CancellationToken cancellationToken)
	{
		await _users.DeleteAsync(id, cancellationToken);
		return Ok(ApiResult.Ok());
	}

	[HttpGet("feature-flags")]
	public async Task<ActionResult<ApiResult<IReadOnlyList<FeatureFlagDto>>>> ListFlags(CancellationToken cancellationToken)
	{
		return Ok(ApiResult<IReadOnlyList<FeatureFlagDto>>.Ok(await _flags.ListAsync(cancellationToken)));
	}

	[HttpPut("feature-flags")]
	public async Task<ActionResult<ApiResult<FeatureFlagDto>>> UpsertFlag([FromBody] UpsertFeatureFlagRequest request, CancellationToken cancellationToken)
	{
		return Ok(ApiResult<FeatureFlagDto>.Ok(await _flags.UpsertAsync(request, cancellationToken)));
	}

	[HttpDelete("feature-flags/{id:guid}")]
	public async Task<ActionResult<ApiResult>> DeleteFlag(Guid id, CancellationToken cancellationToken)
	{
		await _flags.DeleteAsync(id, cancellationToken);
		return Ok(ApiResult.Ok());
	}

	[HttpGet("plans")]
	public async Task<ActionResult<ApiResult<IReadOnlyList<PlanDto>>>> ListPlans(CancellationToken cancellationToken)
	{
		return Ok(ApiResult<IReadOnlyList<PlanDto>>.Ok(await _plans.ListAsync(cancellationToken)));
	}

	[HttpPut("plans")]
	public async Task<ActionResult<ApiResult<PlanDto>>> UpsertPlan([FromBody] UpsertPlanRequest request, CancellationToken cancellationToken)
	{
		return Ok(ApiResult<PlanDto>.Ok(await _plans.UpsertAsync(request, cancellationToken)));
	}

	[HttpDelete("plans/{id:guid}")]
	public async Task<ActionResult<ApiResult>> DeletePlan(Guid id, CancellationToken cancellationToken)
	{
		await _plans.DeleteAsync(id, cancellationToken);
		return Ok(ApiResult.Ok());
	}

	[HttpGet("companies")]
	public async Task<ActionResult<ApiResult<IReadOnlyList<CompanyAdminDto>>>> ListCompanies(CancellationToken cancellationToken)
	{
		return Ok(ApiResult<IReadOnlyList<CompanyAdminDto>>.Ok(await _companies.ListAsync(cancellationToken)));
	}

	[HttpPost("companies")]
	public async Task<ActionResult<ApiResult<CompanyAdminDto>>> CreateCompany([FromBody] UpsertCompanyRequest request, CancellationToken cancellationToken)
	{
		return Ok(ApiResult<CompanyAdminDto>.Ok(await _companies.CreateAsync(request, cancellationToken)));
	}

	[HttpPatch("companies/{id:guid}")]
	public async Task<ActionResult<ApiResult<CompanyAdminDto>>> UpdateCompany(Guid id, [FromBody] UpsertCompanyRequest request, CancellationToken cancellationToken)
	{
		return Ok(ApiResult<CompanyAdminDto>.Ok(await _companies.UpdateAsync(id, request, cancellationToken)));
	}

	[HttpDelete("companies/{id:guid}")]
	public async Task<ActionResult<ApiResult>> DeleteCompany(Guid id, CancellationToken cancellationToken)
	{
		await _companies.DeleteAsync(id, cancellationToken);
		return Ok(ApiResult.Ok());
	}

	[HttpGet("roles")]
	public ActionResult<ApiResult<IReadOnlyList<string>>> ListRoles()
	{
		return Ok(ApiResult<IReadOnlyList<string>>.Ok(Roles.All));
	}
}
