using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Asp.Versioning;
using MADai.Application.Abstractions;
using MADai.Application.Common;
using MADai.Domain.Files;
using MADai.Domain.Tenancy;
using MADai.Shared.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MADai.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/v{version:apiVersion}/files")]
[ApiVersion("1.0")]
public class FilesController : ControllerBase
{
	private readonly IFileStorage _storage;

	private readonly IDbContextAccess _db;

	private readonly ICurrentUserService _currentUser;

	public FilesController(IFileStorage storage, IDbContextAccess db, ICurrentUserService currentUser)
	{
		_storage = storage;
		_db = db;
		_currentUser = currentUser;
	}

	[HttpPost("upload")]
	[RequestSizeLimit(209715200L)]
	public async Task<ActionResult<ApiResult<object>>> Upload(IFormFile file, [FromQuery] string folder = "uploads", CancellationToken cancellationToken = default(CancellationToken))
	{
		if (file == null || file.Length == 0L)
		{
			throw new AppException("File required.");
		}
		Guid companyId = _currentUser.CompanyId ?? throw new ForbiddenException("Company context required.");
		string slug = (await (from c in _db.Companies
			where c.Id == companyId
			select c.Slug).FirstOrDefaultAsync(cancellationToken)) ?? companyId.ToString();
		Stream stream = file.OpenReadStream();
		ActionResult<ApiResult<object>> result;
		try
		{
			StoredFileInfo stored = await _storage.SaveAsync(slug, folder, file.FileName, stream, file.ContentType, cancellationToken);
			FileItem entity = new FileItem
			{
				CompanyId = companyId,
				Name = file.FileName,
				ContentType = file.ContentType,
				SizeBytes = stored.SizeBytes,
				StoragePath = stored.StoragePath,
				StorageProvider = stored.StorageProvider,
				Checksum = stored.Checksum
			};
			_db.Files.Add(entity);
			await _db.SaveChangesAsync(cancellationToken);
			result = Ok(ApiResult<object>.Ok(new { entity.Id, entity.Name, entity.SizeBytes, entity.StoragePath }));
		}
		finally
		{
			if (stream != null)
			{
				await stream.DisposeAsync();
			}
		}
		return result;
	}

	[HttpGet]
	public async Task<ActionResult<ApiResult<IReadOnlyList<object>>>> List(CancellationToken cancellationToken)
	{
		IQueryable<FileItem> query = _db.Files.AsNoTracking().AsQueryable();
		Guid? companyId = _currentUser.CompanyId;
		if (companyId.HasValue)
		{
			Guid cid = companyId.GetValueOrDefault();
			query = query.Where((FileItem f) => f.CompanyId == cid);
		}
		return Ok(ApiResult<IReadOnlyList<object>>.Ok((await (from f in query.OrderByDescending((FileItem f) => f.CreatedDate).Take(200)
			select new { f.Id, f.Name, f.ContentType, f.SizeBytes, f.StoragePath, f.CreatedDate, f.Version }).ToListAsync(cancellationToken)).Cast<object>().ToList()));
	}

	[HttpGet("{id:guid}/download")]
	public async Task<IActionResult> Download(Guid id, CancellationToken cancellationToken)
	{
		FileItem file = (await _db.Files.FirstOrDefaultAsync((FileItem f) => f.Id == id, cancellationToken)) ?? throw new NotFoundException("File", id);
		Guid? companyId = _currentUser.CompanyId;
		if (companyId.HasValue)
		{
			Guid cid = companyId.GetValueOrDefault();
			if (file.CompanyId != cid)
			{
				throw new ForbiddenException();
			}
		}
		return File(await _storage.OpenReadAsync(file.StoragePath, cancellationToken), file.ContentType, file.Name);
	}
}
