using System;
using System.Threading;
using System.Threading.Tasks;
using MADai.Shared.Contracts;

namespace MADai.Application.Features.Auth;

public interface IAuthService
{
	Task<AuthResponse> LoginAsync(LoginRequest request, string? ip, string? userAgent, CancellationToken cancellationToken = default(CancellationToken));

	Task<AuthResponse> RegisterAsync(RegisterRequest request, string? ip, CancellationToken cancellationToken = default(CancellationToken));

	Task<AuthResponse> RefreshAsync(string refreshToken, string? ip, CancellationToken cancellationToken = default(CancellationToken));

	Task LogoutAsync(string refreshToken, string? ip, CancellationToken cancellationToken = default(CancellationToken));

	Task<UserSummary> GetCurrentUserAsync(Guid userId, CancellationToken cancellationToken = default(CancellationToken));

	Task RequestPasswordResetAsync(string email, string resetUrlTemplate, CancellationToken cancellationToken = default(CancellationToken));

	Task ResetPasswordAsync(ResetPasswordRequest request, CancellationToken cancellationToken = default(CancellationToken));

	Task<MfaEnrollResponse> StartMfaEnrollmentAsync(Guid userId, CancellationToken cancellationToken = default(CancellationToken));

	Task<UserSummary> CompleteMfaEnrollmentAsync(Guid userId, string code, CancellationToken cancellationToken = default(CancellationToken));

	Task DisableMfaAsync(Guid userId, string code, CancellationToken cancellationToken = default(CancellationToken));

	Task ChangePasswordAsync(Guid userId, string currentPassword, string newPassword, CancellationToken cancellationToken = default(CancellationToken));

	Task<UserSummary> UpdateProfileAsync(Guid userId, UpdateProfileRequest request, CancellationToken cancellationToken = default(CancellationToken));
}
