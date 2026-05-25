using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MADai.Application.Abstractions;
using MADai.Application.Common;
using MADai.Domain.Enums;
using MADai.Domain.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MADai.Application.Features.TaskTemplates;

public class TaskTemplateService : ITaskTemplateService
{
	private readonly IDbContextAccess _db;

	private readonly ICurrentUserService _currentUser;

	public TaskTemplateService(IDbContextAccess db, ICurrentUserService currentUser)
	{
		_db = db;
		_currentUser = currentUser;
	}

	public async Task<IReadOnlyList<TaskTemplateDto>> ListAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		IQueryable<TaskTemplate> query = _db.TaskTemplates.AsNoTracking().AsQueryable();
		Guid? companyId = _currentUser.CompanyId;
		if (companyId.HasValue)
		{
			Guid cid = companyId.GetValueOrDefault();
			query = query.Where((TaskTemplate t) => t.CompanyId == cid || t.IsPublic);
		}
		return await (from t in query
			orderby t.Name
			select Map(t)).ToListAsync(cancellationToken);
	}

	public async Task<TaskTemplateDto> GetAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
	{
		TaskTemplate t = (await _db.TaskTemplates.AsNoTracking().FirstOrDefaultAsync((TaskTemplate x) => x.Id == id, cancellationToken)) ?? throw new NotFoundException("TaskTemplate", id);
		Guid? companyId = _currentUser.CompanyId;
		if (companyId.HasValue)
		{
			Guid cid = companyId.GetValueOrDefault();
			if (t.CompanyId != cid && !t.IsPublic)
			{
				throw new ForbiddenException();
			}
		}
		return Map(t);
	}

	public async Task<TaskTemplateDto> CreateAsync(CreateTaskTemplateRequest request, CancellationToken cancellationToken = default(CancellationToken))
	{
		Guid companyId = _currentUser.CompanyId ?? throw new ForbiddenException();
		if (string.IsNullOrWhiteSpace(request.Name))
		{
			throw new AppException("Name required.");
		}
		TaskTemplate entity = new TaskTemplate
		{
			CompanyId = companyId,
			Name = request.Name.Trim(),
			Description = request.Description,
			Category = request.Category,
			DefaultPriority = request.DefaultPriority.GetValueOrDefault(TaskPriority.Normal),
			PromptTemplate = request.PromptTemplate,
			DefaultInputJson = request.DefaultInputJson,
			QueueName = request.Queue,
			DefaultTimeoutSeconds = request.DefaultTimeoutSeconds.GetValueOrDefault(3600),
			DefaultMaxRetries = request.DefaultMaxRetries.GetValueOrDefault(3),
			IsPublic = request.IsPublic
		};
		_db.TaskTemplates.Add(entity);
		await _db.SaveChangesAsync(cancellationToken);
		return Map(entity);
	}

	public async Task<TaskTemplateDto> UpdateAsync(Guid id, UpdateTaskTemplateRequest request, CancellationToken cancellationToken = default(CancellationToken))
	{
		TaskTemplate entity = (await _db.TaskTemplates.FirstOrDefaultAsync((TaskTemplate t) => t.Id == id, cancellationToken)) ?? throw new NotFoundException("TaskTemplate", id);
		Guid? companyId = _currentUser.CompanyId;
		if (companyId.HasValue)
		{
			Guid cid = companyId.GetValueOrDefault();
			if (entity.CompanyId != cid)
			{
				throw new ForbiddenException();
			}
		}
		string i = request.Name;
		if (i != null && i.Length > 0)
		{
			entity.Name = i.Trim();
		}
		if (request.Description != null)
		{
			entity.Description = request.Description;
		}
		TaskCategory? category = request.Category;
		if (category.HasValue)
		{
			TaskCategory c = category.GetValueOrDefault();
			entity.Category = c;
		}
		TaskPriority? defaultPriority = request.DefaultPriority;
		if (defaultPriority.HasValue)
		{
			TaskPriority p = defaultPriority.GetValueOrDefault();
			entity.DefaultPriority = p;
		}
		if (request.PromptTemplate != null)
		{
			entity.PromptTemplate = request.PromptTemplate;
		}
		if (request.DefaultInputJson != null)
		{
			entity.DefaultInputJson = request.DefaultInputJson;
		}
		if (request.Queue != null)
		{
			entity.QueueName = request.Queue;
		}
		int? defaultTimeoutSeconds = request.DefaultTimeoutSeconds;
		if (defaultTimeoutSeconds.HasValue)
		{
			int t2 = defaultTimeoutSeconds.GetValueOrDefault();
			entity.DefaultTimeoutSeconds = t2;
		}
		defaultTimeoutSeconds = request.DefaultMaxRetries;
		if (defaultTimeoutSeconds.HasValue)
		{
			int r = defaultTimeoutSeconds.GetValueOrDefault();
			entity.DefaultMaxRetries = r;
		}
		bool? isPublic = request.IsPublic;
		if (isPublic.HasValue)
		{
			bool pub = isPublic.GetValueOrDefault();
			entity.IsPublic = pub;
		}
		await _db.SaveChangesAsync(cancellationToken);
		return Map(entity);
	}

	public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
	{
		TaskTemplate entity = (await _db.TaskTemplates.FirstOrDefaultAsync((TaskTemplate t) => t.Id == id, cancellationToken)) ?? throw new NotFoundException("TaskTemplate", id);
		Guid? companyId = _currentUser.CompanyId;
		if (companyId.HasValue)
		{
			Guid cid = companyId.GetValueOrDefault();
			if (entity.CompanyId != cid)
			{
				throw new ForbiddenException();
			}
		}
		_db.TaskTemplates.Remove(entity);
		await _db.SaveChangesAsync(cancellationToken);
	}

	private static TaskTemplateDto Map(TaskTemplate t)
	{
		return new TaskTemplateDto(t.Id, t.Name, t.Description, t.Category, t.DefaultPriority, t.PromptTemplate, t.DefaultInputJson, t.QueueName, t.DefaultTimeoutSeconds, t.DefaultMaxRetries, t.IsPublic, t.CreatedDate);
	}
}
