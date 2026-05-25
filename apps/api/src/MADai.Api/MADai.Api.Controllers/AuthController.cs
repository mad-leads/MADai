using System;
using System.Threading;
using System.Threading.Tasks;
using Asp.Versioning;
using MADai.Application.Abstractions;
using MADai.Application.Features.Auth;
using MADai.Shared.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MADai.Api.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/auth")]
[ApiVersion("1.0")]
public class AuthController : ControllerBase
{
	private readonly IAuthService _authService;

	private readonly ICurrentUserService _currentUser;

	public AuthController(IAuthService authService, ICurrentUserService currentUser)
	{
		_authService = authService;
		_currentUser = currentUser;
	}

	[HttpPost("login")]
	[AllowAnonymous]
	public async Task<ActionResult<ApiResult<AuthResponse>>> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
	{
		string ip = base.HttpContext.Connection.RemoteIpAddress?.ToString();
		string ua = base.Request.Headers.UserAgent.ToString();
		return Ok(ApiResult<AuthResponse>.Ok(await _authService.LoginAsync(request, ip, ua, cancellationToken)));
	}

	[HttpPost("register")]
	[AllowAnonymous]
	public async Task<ActionResult<ApiResult<AuthResponse>>> Register([FromBody] RegisterRequest request, CancellationToken cancellationToken)
	{
		string ip = base.HttpContext.Connection.RemoteIpAddress?.ToString();
		return Ok(ApiResult<AuthResponse>.Ok(await _authService.RegisterAsync(request, ip, cancellationToken)));
	}

	[HttpPost("refresh")]
	[AllowAnonymous]
	public async Task<ActionResult<ApiResult<AuthResponse>>> Refresh([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
	{
		string ip = base.HttpContext.Connection.RemoteIpAddress?.ToString();
		return Ok(ApiResult<AuthResponse>.Ok(await _authService.RefreshAsync(request.RefreshToken, ip, cancellationToken)));
	}

	[HttpPost("logout")]
	[Authorize]
	public async Task<ActionResult<ApiResult>> Logout([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
	{
		string ip = base.HttpContext.Connection.RemoteIpAddress?.ToString();
		await _authService.LogoutAsync(request.RefreshToken, ip, cancellationToken);
		return Ok(ApiResult.Ok());
	}

	[HttpGet("me")]
	[Authorize]
	public async Task<ActionResult<ApiResult<UserSummary>>> Me(CancellationToken cancellationToken)
	{
		Guid userId = _currentUser.UserId ?? throw new UnauthorizedAccessException();
		return Ok(ApiResult<UserSummary>.Ok(await _authService.GetCurrentUserAsync(userId, cancellationToken)));
	}

	[HttpPost("forgot-password")]
	[AllowAnonymous]
	public async Task<ActionResult<ApiResult>> ForgotPassword([FromBody] ForgotPasswordRequest request, CancellationToken cancellationToken)
	{
		string template = base.Request.Headers["X-Reset-Url-Template"].ToString();
		if (string.IsNullOrWhiteSpace(template))
		{
			template = "https://madai.madproducts.com/auth/reset?email={email}&token={token}";
		}
		await _authService.RequestPasswordResetAsync(request.Email, template, cancellationToken);
		return Ok(ApiResult.Ok());
	}

	[HttpPost("reset-password")]
	[AllowAnonymous]
	public async Task<ActionResult<ApiResult>> ResetPassword([FromBody] ResetPasswordRequest request, CancellationToken cancellationToken)
	{
		await _authService.ResetPasswordAsync(request, cancellationToken);
		return Ok(ApiResult.Ok());
	}

	[HttpPost("mfa/start")]
	[Authorize]
	public async Task<ActionResult<ApiResult<MfaEnrollResponse>>> MfaStart(CancellationToken cancellationToken)
	{
		Guid userId = _currentUser.UserId ?? throw new UnauthorizedAccessException();
		return Ok(ApiResult<MfaEnrollResponse>.Ok(await _authService.StartMfaEnrollmentAsync(userId, cancellationToken)));
	}

	[HttpPost("mfa/verify")]
	[Authorize]
	public async Task<ActionResult<ApiResult<UserSummary>>> MfaVerify([FromBody] MfaSetupRequest request, CancellationToken cancellationToken)
	{
		Guid userId = _currentUser.UserId ?? throw new UnauthorizedAccessException();
		return Ok(ApiResult<UserSummary>.Ok(await _authService.CompleteMfaEnrollmentAsync(userId, request.Code, cancellationToken)));
	}

	[HttpPost("mfa/disable")]
	[Authorize]
	public async Task<ActionResult<ApiResult>> MfaDisable([FromBody] MfaSetupRequest request, CancellationToken cancellationToken)
	{
		Guid userId = _currentUser.UserId ?? throw new UnauthorizedAccessException();
		await _authService.DisableMfaAsync(userId, request.Code, cancellationToken);
		return Ok(ApiResult.Ok());
	}

	[HttpPost("change-password")]
	[Authorize]
	public async Task<ActionResult<ApiResult>> ChangePassword([FromBody] ChangePasswordRequest request, CancellationToken cancellationToken)
	{
		Guid userId = _currentUser.UserId ?? throw new UnauthorizedAccessException();
		await _authService.ChangePasswordAsync(userId, request.CurrentPassword, request.NewPassword, cancellationToken);
		return Ok(ApiResult.Ok());
	}

	[HttpPut("profile")]
	[Authorize]
	public async Task<ActionResult<ApiResult<UserSummary>>> UpdateProfile([FromBody] UpdateProfileRequest request, CancellationToken cancellationToken)
	{
		Guid userId = _currentUser.UserId ?? throw new UnauthorizedAccessException();
		return Ok(ApiResult<UserSummary>.Ok(await _authService.UpdateProfileAsync(userId, request, cancellationToken)));
	}
}
