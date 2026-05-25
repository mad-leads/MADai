using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MADai.Application.Abstractions;
using MADai.Application.Common;
using MADai.Domain.Tenancy;
using Microsoft.EntityFrameworkCore;

namespace MADai.Application.Features.Admin;

public class CompanyAdminService : ICompanyAdminService
{
	private readonly IDbContextAccess _db;

	public CompanyAdminService(IDbContextAccess db)
	{
		_db = db;
	}

	public async Task<IReadOnlyList<CompanyAdminDto>> ListAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		return await (from c in _db.Companies.AsNoTracking()
			orderby c.Name
			select new CompanyAdminDto(c.Id, c.Name, c.Slug, c.ContactEmail, c.IsActive, c.CreatedDate)).ToListAsync(cancellationToken);
	}

	public async Task<CompanyAdminDto> CreateAsync(UpsertCompanyRequest request, CancellationToken cancellationToken = default(CancellationToken))
	{
		UpsertCompanyRequest request2 = request;
		if (await _db.Companies.AnyAsync((Company c) => c.Slug == request2.Slug, cancellationToken))
		{
			throw new ConflictException("Company with slug '" + request2.Slug + "' already exists.");
		}
		Company entity = new Company
		{
			Name = request2.Name,
			Slug = request2.Slug,
			ContactEmail = request2.ContactEmail,
			IsActive = request2.IsActive,
			Branding = new CompanyBranding(),
			Settings = new CompanySettings()
		};
		_db.Companies.Add(entity);
		await _db.SaveChangesAsync(cancellationToken);
		return new CompanyAdminDto(entity.Id, entity.Name, entity.Slug, entity.ContactEmail, entity.IsActive, entity.CreatedDate);
	}

	public async Task<CompanyAdminDto> UpdateAsync(Guid id, UpsertCompanyRequest request, CancellationToken cancellationToken = default(CancellationToken))
	{
		Company entity = (await _db.Companies.FirstOrDefaultAsync((Company c) => c.Id == id, cancellationToken)) ?? throw new NotFoundException("Company", id);
		entity.Name = request.Name;
		entity.Slug = request.Slug;
		entity.ContactEmail = request.ContactEmail;
		entity.IsActive = request.IsActive;
		await _db.SaveChangesAsync(cancellationToken);
		return new CompanyAdminDto(entity.Id, entity.Name, entity.Slug, entity.ContactEmail, entity.IsActive, entity.CreatedDate);
	}

	public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
	{
		Company entity = (await _db.Companies.FirstOrDefaultAsync((Company c) => c.Id == id, cancellationToken)) ?? throw new NotFoundException("Company", id);
		_db.Companies.Remove(entity);
		await _db.SaveChangesAsync(cancellationToken);
	}
}
