using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MADai.Application.Abstractions;
using MADai.Application.Common;
using MADai.Domain.Identity;
using MADai.Domain.Tenancy;
using MADai.Shared.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MADai.Application.Features.Admin;

public class AdminUserService : IAdminUserService
{
	private readonly UserManager<ApplicationUser> _userManager;

	private readonly RoleManager<ApplicationRole> _roleManager;

	private readonly IDbContextAccess _db;

	public AdminUserService(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IDbContextAccess db)
	{
		_userManager = userManager;
		_roleManager = roleManager;
		_db = db;
	}

	public async Task<PagedResult<AdminUserDto>> ListAsync(PageQuery query, CancellationToken cancellationToken = default(CancellationToken))
	{
		PageQuery r = query.Normalize();
		IQueryable<ApplicationUser> q = _db.Users.AsNoTracking().AsQueryable();
		if (!string.IsNullOrWhiteSpace(r.Search))
		{
			string term = r.Search.Trim();
			q = q.Where((ApplicationUser u) => EF.Functions.Like(u.Email, $"%{term}%") || (u.FirstName != null && EF.Functions.Like(u.FirstName, $"%{term}%")) || (u.LastName != null && EF.Functions.Like(u.LastName, $"%{term}%")));
		}
		long total = await q.LongCountAsync(cancellationToken);
		List<ApplicationUser> users = await q.OrderByDescending((ApplicationUser u) => u.LastLoginAt ?? u.CreatedDate).Skip((r.Page - 1) * r.PageSize).Take(r.PageSize)
			.ToListAsync(cancellationToken);
		Dictionary<Guid, List<string>> rolesByUser = new Dictionary<Guid, List<string>>();
		foreach (ApplicationUser u2 in users)
		{
			IList<string> rs = await _userManager.GetRolesAsync(u2);
			rolesByUser[u2.Id] = rs.ToList();
		}
		List<Guid> companyIds = (from u in users
			where u.CompanyId.HasValue
			select u.CompanyId.Value).Distinct().ToList();
		Dictionary<Guid, string> companies = await (from c in _db.Companies.AsNoTracking()
			where companyIds.Contains(c.Id)
			select new { c.Id, c.Name }).ToDictionaryAsync(c => c.Id, c => c.Name, cancellationToken);
		string value;
		List<string> value2;
		return new PagedResult<AdminUserDto>(users.Select((ApplicationUser u) => new AdminUserDto(u.Id, u.Email ?? "", u.FirstName, u.LastName, u.IsActive, u.IsMfaEnrolled, u.CompanyId, (u.CompanyId.HasValue && companies.TryGetValue(u.CompanyId.Value, out value)) ? value : null, rolesByUser.TryGetValue(u.Id, out value2) ? value2 : new List<string>(), u.LastLoginAt, u.CreatedDate)).ToList(), r.Page, r.PageSize, total);
	}

	public async Task<AdminUserDto> GetAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
	{
		AdminUserDto match = (await ListAsync(new PageQuery
		{
			Page = 1,
			PageSize = 1,
			Search = id.ToString()
		}, cancellationToken)).Items.FirstOrDefault((AdminUserDto u) => u.Id == id);
		if ((object)match != null)
		{
			return match;
		}
		ApplicationUser user = (await _userManager.FindByIdAsync(id.ToString())) ?? throw new NotFoundException("User", id);
		IList<string> rs = await _userManager.GetRolesAsync(user);
		return new AdminUserDto(user.Id, user.Email ?? "", user.FirstName, user.LastName, user.IsActive, user.IsMfaEnrolled, user.CompanyId, null, rs.ToList(), user.LastLoginAt, user.CreatedDate);
	}

	public async Task<AdminUserDto> CreateAsync(CreateAdminUserRequest request, CancellationToken cancellationToken = default(CancellationToken))
	{
		if (await _userManager.FindByEmailAsync(request.Email) != null)
		{
			throw new ConflictException("Email already exists.");
		}
		ApplicationUser user = new ApplicationUser
		{
			UserName = request.Email,
			Email = request.Email,
			EmailConfirmed = true,
			IsActive = true,
			FirstName = request.FirstName,
			LastName = request.LastName,
			CompanyId = request.CompanyId
		};
		IdentityResult result = await _userManager.CreateAsync(user, request.Password);
		if (!result.Succeeded)
		{
			throw new ValidationFailedException(result.Errors.Select((IdentityError e) => e.Description));
		}
		foreach (string role in request.Roles ?? Array.Empty<string>())
		{
			if (await _roleManager.RoleExistsAsync(role))
			{
				await _userManager.AddToRoleAsync(user, role);
			}
		}
		return await GetAsync(user.Id, cancellationToken);
	}

	public async Task<AdminUserDto> UpdateAsync(Guid id, UpdateAdminUserRequest request, CancellationToken cancellationToken = default(CancellationToken))
	{
		ApplicationUser user = (await _userManager.FindByIdAsync(id.ToString())) ?? throw new NotFoundException("User", id);
		if (request.FirstName != null)
		{
			user.FirstName = request.FirstName;
		}
		if (request.LastName != null)
		{
			user.LastName = request.LastName;
		}
		bool? isActive = request.IsActive;
		if (isActive.HasValue)
		{
			bool a = isActive.GetValueOrDefault();
			user.IsActive = a;
		}
		Guid? companyId = request.CompanyId;
		if (companyId.HasValue)
		{
			Guid c = companyId.GetValueOrDefault();
			user.CompanyId = c;
		}
		IdentityResult update = await _userManager.UpdateAsync(user);
		if (!update.Succeeded)
		{
			throw new ValidationFailedException(update.Errors.Select((IdentityError e) => e.Description));
		}
		if (request.Roles != null)
		{
			IList<string> existing = await _userManager.GetRolesAsync(user);
			string[] toAdd = request.Roles.Except(existing).ToArray();
			string[] toRemove = existing.Except(request.Roles).ToArray();
			if (toRemove.Length != 0)
			{
				await _userManager.RemoveFromRolesAsync(user, toRemove);
			}
			if (toAdd.Length != 0)
			{
				await _userManager.AddToRolesAsync(user, toAdd);
			}
		}
		return await GetAsync(user.Id, cancellationToken);
	}

	public async Task SetPasswordAsync(Guid id, string newPassword, CancellationToken cancellationToken = default(CancellationToken))
	{
		ApplicationUser user = (await _userManager.FindByIdAsync(id.ToString())) ?? throw new NotFoundException("User", id);
		string token = await _userManager.GeneratePasswordResetTokenAsync(user);
		IdentityResult result = await _userManager.ResetPasswordAsync(user, token, newPassword);
		if (!result.Succeeded)
		{
			throw new ValidationFailedException(result.Errors.Select((IdentityError e) => e.Description));
		}
	}

	public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
	{
		ApplicationUser user = (await _userManager.FindByIdAsync(id.ToString())) ?? throw new NotFoundException("User", id);
		user.IsActive = false;
		user.IsDeleted = true;
		user.DeletedDate = DateTime.UtcNow;
		IdentityResult result = await _userManager.UpdateAsync(user);
		if (!result.Succeeded)
		{
			throw new ValidationFailedException(result.Errors.Select((IdentityError e) => e.Description));
		}
	}
}
