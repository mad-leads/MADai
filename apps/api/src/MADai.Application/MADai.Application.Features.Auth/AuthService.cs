using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MADai.Application.Abstractions;
using MADai.Application.Common;
using MADai.Domain.Identity;
using MADai.Domain.Tenancy;
using MADai.Shared.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MADai.Application.Features.Auth;

public class AuthService : IAuthService
{
	private readonly UserManager<ApplicationUser> _userManager;

	private readonly SignInManager<ApplicationUser> _signInManager;

	private readonly RoleManager<ApplicationRole> _roleManager;

	private readonly IJwtTokenService _jwt;

	private readonly IDbContextAccess _db;

	private readonly IDateTimeProvider _clock;

	private readonly IEmailSender _email;

	private readonly ILogger<AuthService> _logger;

	public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager, IJwtTokenService jwt, IDbContextAccess db, IDateTimeProvider clock, IEmailSender email, ILogger<AuthService> logger)
	{
		_userManager = userManager;
		_signInManager = signInManager;
		_roleManager = roleManager;
		_jwt = jwt;
		_db = db;
		_clock = clock;
		_email = email;
		_logger = logger;
	}

	public async Task<AuthResponse> LoginAsync(LoginRequest request, string? ip, string? userAgent, CancellationToken cancellationToken = default(CancellationToken))
	{
		ApplicationUser user = await _userManager.FindByEmailAsync(request.Email);
		if (user == null || !user.IsActive)
		{
			await LogLogin(null, ip, userAgent, success: false, "Invalid credentials", cancellationToken);
			throw new AppException("Invalid credentials.");
		}
		SignInResult check = await _signInManager.CheckPasswordSignInAsync(user, request.Password, lockoutOnFailure: true);
		if (!check.Succeeded)
		{
			await LogLogin(user.Id, ip, userAgent, success: false, check.IsLockedOut ? "Locked out" : "Invalid credentials", cancellationToken);
			throw new AppException(check.IsLockedOut ? "Account locked." : "Invalid credentials.");
		}
		if (user.IsMfaEnrolled)
		{
			if (string.IsNullOrWhiteSpace(request.MfaCode))
			{
				throw new AppException("MFA code required.");
			}
			if (!(await _userManager.VerifyTwoFactorTokenAsync(user, TokenOptions.DefaultAuthenticatorProvider, request.MfaCode)))
			{
				throw new AppException("Invalid MFA code.");
			}
		}
		user.LastLoginAt = _clock.UtcNow;
		user.LastLoginIp = ip;
		await _userManager.UpdateAsync(user);
		await LogLogin(user.Id, ip, userAgent, success: true, null, cancellationToken);
		return await IssueTokensAsync(user, ip, cancellationToken);
	}

	public async Task<AuthResponse> RegisterAsync(RegisterRequest request, string? ip, CancellationToken cancellationToken = default(CancellationToken))
	{
		if (await _userManager.FindByEmailAsync(request.Email) != null)
		{
			throw new ConflictException("Email already registered.");
		}
		Guid? companyId = null;
		if (!string.IsNullOrWhiteSpace(request.CompanyName))
		{
			Company company = new Company
			{
				Name = request.CompanyName,
				Slug = Slugify(request.CompanyName),
				ContactEmail = request.Email,
				Branding = new CompanyBranding(),
				Settings = new CompanySettings()
			};
			_db.Companies.Add(company);
			await _db.SaveChangesAsync(cancellationToken);
			companyId = company.Id;
		}
		ApplicationUser user = new ApplicationUser
		{
			UserName = request.Email,
			Email = request.Email,
			EmailConfirmed = false,
			FirstName = request.FirstName,
			LastName = request.LastName,
			CompanyId = companyId,
			IsActive = true
		};
		IdentityResult result = await _userManager.CreateAsync(user, request.Password);
		if (!result.Succeeded)
		{
			throw new ValidationFailedException(result.Errors.Select((IdentityError e) => e.Description));
		}
		string role = ((!companyId.HasValue) ? "User" : "CompanyAdmin");
		if (await _roleManager.RoleExistsAsync(role))
		{
			await _userManager.AddToRoleAsync(user, role);
		}
		return await IssueTokensAsync(user, ip, cancellationToken);
	}

	public async Task<AuthResponse> RefreshAsync(string refreshToken, string? ip, CancellationToken cancellationToken = default(CancellationToken))
	{
		string refreshToken2 = refreshToken;
		RefreshToken token = await _db.RefreshTokens.Include((RefreshToken t) => t.User).FirstOrDefaultAsync((RefreshToken t) => t.Token == refreshToken2, cancellationToken);
		if (token == null || !token.IsActive)
		{
			throw new AppException("Invalid refresh token.");
		}
		string newToken = _jwt.GenerateRefreshToken();
		token.RevokedAt = _clock.UtcNow;
		token.RevokedByIp = ip;
		token.ReplacedByToken = newToken;
		token.ReasonRevoked = "Rotation";
		RefreshToken newEntity = new RefreshToken
		{
			UserId = token.UserId,
			Token = newToken,
			CreatedAt = _clock.UtcNow,
			ExpiresAt = _clock.UtcNow.AddDays(30.0),
			CreatedByIp = ip
		};
		_db.RefreshTokens.Add(newEntity);
		await _db.SaveChangesAsync(cancellationToken);
		ApplicationUser user = token.User;
		IList<string> roles = await _userManager.GetRolesAsync(user);
		IReadOnlyList<string> permissions = await GetPermissionCodesAsync(roles, cancellationToken);
		(string, DateTime) obj = await _jwt.GenerateAccessTokenAsync(user, roles, permissions, cancellationToken);
		string access = obj.Item1;
		DateTime exp = obj.Item2;
		string accessToken = access;
		string refreshToken3 = newToken;
		DateTime expiresAt = exp;
		return new AuthResponse(accessToken, refreshToken3, expiresAt, await BuildUserSummary(user, cancellationToken));
	}

	public async Task LogoutAsync(string refreshToken, string? ip, CancellationToken cancellationToken = default(CancellationToken))
	{
		string refreshToken2 = refreshToken;
		RefreshToken token = await _db.RefreshTokens.FirstOrDefaultAsync((RefreshToken t) => t.Token == refreshToken2, cancellationToken);
		if (token != null && token.IsActive)
		{
			token.RevokedAt = _clock.UtcNow;
			token.RevokedByIp = ip;
			token.ReasonRevoked = "Logout";
			await _db.SaveChangesAsync(cancellationToken);
		}
	}

	public async Task<UserSummary> GetCurrentUserAsync(Guid userId, CancellationToken cancellationToken = default(CancellationToken))
	{
		ApplicationUser user = (await _userManager.FindByIdAsync(userId.ToString())) ?? throw new NotFoundException("User", userId);
		return await BuildUserSummary(user, cancellationToken);
	}

	private async Task<AuthResponse> IssueTokensAsync(ApplicationUser user, string? ip, CancellationToken cancellationToken)
	{
		IList<string> roles = await _userManager.GetRolesAsync(user);
		IReadOnlyList<string> permissions = await GetPermissionCodesAsync(roles, cancellationToken);
		(string, DateTime) tuple = await _jwt.GenerateAccessTokenAsync(user, roles, permissions, cancellationToken);
		string access = tuple.Item1;
		DateTime exp = tuple.Item2;
		string refresh = _jwt.GenerateRefreshToken();
		_db.RefreshTokens.Add(new RefreshToken
		{
			UserId = user.Id,
			Token = refresh,
			ExpiresAt = _clock.UtcNow.AddDays(30.0),
			CreatedByIp = ip
		});
		await _db.SaveChangesAsync(cancellationToken);
		string accessToken = access;
		string refreshToken = refresh;
		DateTime expiresAt = exp;
		return new AuthResponse(accessToken, refreshToken, expiresAt, await BuildUserSummary(user, cancellationToken));
	}

	private async Task<UserSummary> BuildUserSummary(ApplicationUser user, CancellationToken cancellationToken)
	{
		ApplicationUser user2 = user;
		IList<string> roles = await _userManager.GetRolesAsync(user2);
		IReadOnlyList<string> permissions = await GetPermissionCodesAsync(roles, cancellationToken);
		string text = ((!user2.CompanyId.HasValue) ? null : (await (from c in _db.Companies
			where c.Id == user2.CompanyId
			select c.Name).FirstOrDefaultAsync(cancellationToken)));
		string companyName = text;
		return new UserSummary(user2.Id, user2.Email, user2.DisplayName, user2.CompanyId, companyName, roles.ToList(), permissions.ToList(), user2.IsMfaEnrolled, user2.AvatarUrl, user2.FirstName, user2.LastName, user2.TimeZone, user2.Locale);
	}

	private async Task<IReadOnlyList<string>> GetPermissionCodesAsync(IList<string> roleNames, CancellationToken cancellationToken)
	{
		IList<string> roleNames2 = roleNames;
		if (roleNames2.Count == 0)
		{
			return Array.Empty<string>();
		}
		List<Guid> roleIds = await (from r in _db.Roles
			where roleNames2.Contains(r.Name)
			select r.Id).ToListAsync(cancellationToken);
		return await (from rp in _db.RolePermissions
			where roleIds.Contains(rp.RoleId) && rp.Permission != null
			select rp.Permission.Code).Distinct().ToListAsync(cancellationToken);
	}

	private async Task LogLogin(Guid? userId, string? ip, string? userAgent, bool success, string? reason, CancellationToken cancellationToken)
	{
		if (!userId.HasValue)
		{
			return;
		}
		_db.LoginHistory.Add(new LoginHistory
		{
			UserId = userId.Value,
			IpAddress = ip,
			UserAgent = userAgent,
			LoginAt = _clock.UtcNow,
			Success = success,
			FailureReason = reason
		});
		try
		{
			await _db.SaveChangesAsync(cancellationToken);
		}
		catch (Exception ex)
		{
			_logger.LogWarning(ex, "Failed to save login history");
		}
	}

	private static string Slugify(string s)
	{
		return new string((from c in s.ToLowerInvariant()
			select (!char.IsLetterOrDigit(c)) ? '-' : c).ToArray()).Trim('-');
	}

	public async Task RequestPasswordResetAsync(string email, string resetUrlTemplate, CancellationToken cancellationToken = default(CancellationToken))
	{
		ApplicationUser user = await _userManager.FindByEmailAsync(email);
		if (user == null || !user.IsActive)
		{
			_logger.LogInformation("Password reset for unknown email {Email} - silently ignored.", email);
			return;
		}
		string token = await _userManager.GeneratePasswordResetTokenAsync(user);
		string encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
		string link = (resetUrlTemplate ?? "/auth/reset?email={email}&token={token}").Replace("{email}", WebUtility.UrlEncode(user.Email ?? "")).Replace("{token}", encodedToken);
		string html = $"<p>Hi {WebUtility.HtmlEncode(user.DisplayName)},</p>\n<p>A password reset was requested for your MADai account. Click the link below within 30 minutes to set a new password:</p>\n<p><a href='{link}'>Reset your password</a></p>\n<p>If you didn't request this, ignore this email.</p>";
		try
		{
			await _email.SendAsync(user.Email, "MADai - Reset your password", html, cancellationToken);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Failed to send password reset email to {Email}", user.Email);
		}
	}

	public async Task ResetPasswordAsync(ResetPasswordRequest request, CancellationToken cancellationToken = default(CancellationToken))
	{
		ApplicationUser user = (await _userManager.FindByEmailAsync(request.Email)) ?? throw new AppException("Invalid reset link.");
		string decodedToken;
		try
		{
			decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token));
		}
		catch
		{
			throw new AppException("Invalid reset link.");
		}
		IdentityResult result = await _userManager.ResetPasswordAsync(user, decodedToken, request.NewPassword);
		if (!result.Succeeded)
		{
			throw new ValidationFailedException(result.Errors.Select((IdentityError e) => e.Description));
		}
		await _userManager.UpdateSecurityStampAsync(user);
	}

	public async Task<MfaEnrollResponse> StartMfaEnrollmentAsync(Guid userId, CancellationToken cancellationToken = default(CancellationToken))
	{
		ApplicationUser user = (await _userManager.FindByIdAsync(userId.ToString())) ?? throw new NotFoundException("User", userId);
		await _userManager.ResetAuthenticatorKeyAsync(user);
		string key = (await _userManager.GetAuthenticatorKeyAsync(user)) ?? throw new AppException("Could not provision authenticator key.");
		string issuer = "MADai";
		string account = WebUtility.UrlEncode(user.Email ?? user.Id.ToString());
		string uri = $"otpauth://totp/{issuer}:{account}?secret={key}&issuer={issuer}&digits=6";
		return new MfaEnrollResponse(key, uri);
	}

	public async Task<UserSummary> CompleteMfaEnrollmentAsync(Guid userId, string code, CancellationToken cancellationToken = default(CancellationToken))
	{
		ApplicationUser user = (await _userManager.FindByIdAsync(userId.ToString())) ?? throw new NotFoundException("User", userId);
		if (!(await _userManager.VerifyTwoFactorTokenAsync(user, TokenOptions.DefaultAuthenticatorProvider, code)))
		{
			throw new AppException("Invalid MFA code.");
		}
		await _userManager.SetTwoFactorEnabledAsync(user, enabled: true);
		user.IsMfaEnrolled = true;
		await _userManager.UpdateAsync(user);
		return await BuildUserSummary(user, cancellationToken);
	}

	public async Task DisableMfaAsync(Guid userId, string code, CancellationToken cancellationToken = default(CancellationToken))
	{
		ApplicationUser user = (await _userManager.FindByIdAsync(userId.ToString())) ?? throw new NotFoundException("User", userId);
		if (!(await _userManager.VerifyTwoFactorTokenAsync(user, TokenOptions.DefaultAuthenticatorProvider, code)))
		{
			throw new AppException("Invalid MFA code.");
		}
		await _userManager.SetTwoFactorEnabledAsync(user, enabled: false);
		await _userManager.ResetAuthenticatorKeyAsync(user);
		user.IsMfaEnrolled = false;
		await _userManager.UpdateAsync(user);
	}

	public async Task ChangePasswordAsync(Guid userId, string currentPassword, string newPassword, CancellationToken cancellationToken = default(CancellationToken))
	{
		ApplicationUser user = (await _userManager.FindByIdAsync(userId.ToString())) ?? throw new NotFoundException("User", userId);
		IdentityResult result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
		if (!result.Succeeded)
		{
			throw new ValidationFailedException(result.Errors.Select((IdentityError e) => e.Description));
		}
	}

	public async Task<UserSummary> UpdateProfileAsync(Guid userId, UpdateProfileRequest request, CancellationToken cancellationToken = default(CancellationToken))
	{
		ApplicationUser user = (await _userManager.FindByIdAsync(userId.ToString())) ?? throw new NotFoundException("User", userId);
		if (request.FirstName != null)
		{
			user.FirstName = request.FirstName;
		}
		if (request.LastName != null)
		{
			user.LastName = request.LastName;
		}
		if (request.AvatarUrl != null)
		{
			user.AvatarUrl = request.AvatarUrl;
		}
		if (request.TimeZone != null)
		{
			user.TimeZone = request.TimeZone;
		}
		if (request.Locale != null)
		{
			user.Locale = request.Locale;
		}
		IdentityResult result = await _userManager.UpdateAsync(user);
		if (!result.Succeeded)
		{
			throw new ValidationFailedException(result.Errors.Select((IdentityError e) => e.Description));
		}
		return await BuildUserSummary(user, cancellationToken);
	}
}
