using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MADai.Application.Abstractions;
using MADai.Application.Common;
using MADai.Domain.Tasks;
using MADai.Shared.Contracts;
using Microsoft.EntityFrameworkCore;

namespace MADai.Application.Features.ClaudePromptTemplates;

public class ClaudePromptTemplateService : IClaudePromptTemplateService
{
	private readonly IDbContextAccess _db;

	public ClaudePromptTemplateService(IDbContextAccess db)
	{
		_db = db;
	}

	public async Task<IReadOnlyList<ClaudePromptTemplateDto>> ListAsync(CancellationToken cancellationToken = default(CancellationToken))
	{
		return await (from t in _db.ClaudePromptTemplates.AsNoTracking()
			orderby t.Name
			select new ClaudePromptTemplateDto(t.Id, t.Name, t.Description, t.Content, t.CreatedDate, t.ModifiedDate)).ToListAsync(cancellationToken);
	}

	public async Task<ClaudePromptTemplateDto> CreateAsync(CreateClaudePromptTemplateRequest request, CancellationToken cancellationToken = default(CancellationToken))
	{
		if (string.IsNullOrWhiteSpace(request.Name))
		{
			throw new AppException("Template name is required.");
		}
		if (string.IsNullOrWhiteSpace(request.Content))
		{
			throw new AppException("Template content is required.");
		}
		string trimmedName = request.Name.Trim();
		if (await _db.ClaudePromptTemplates.AnyAsync((ClaudePromptTemplate t) => t.Name == trimmedName, cancellationToken))
		{
			throw new ConflictException("A template with the name '" + trimmedName + "' already exists.");
		}
		ClaudePromptTemplate entity = new ClaudePromptTemplate
		{
			Name = trimmedName,
			Description = request.Description,
			Content = request.Content
		};
		_db.ClaudePromptTemplates.Add(entity);
		await _db.SaveChangesAsync(cancellationToken);
		return new ClaudePromptTemplateDto(entity.Id, entity.Name, entity.Description, entity.Content, entity.CreatedDate, entity.ModifiedDate);
	}

	public async Task<ClaudePromptTemplateDto> UpdateAsync(Guid id, UpdateClaudePromptTemplateRequest request, CancellationToken cancellationToken = default(CancellationToken))
	{
		ClaudePromptTemplate entity = (await _db.ClaudePromptTemplates.FirstOrDefaultAsync((ClaudePromptTemplate t) => t.Id == id, cancellationToken)) ?? throw new NotFoundException("ClaudePromptTemplate", id);
		if (request.Name != null && request.Name.Length > 0)
		{
			string name = request.Name.Trim();
			bool flag = name != entity.Name;
			if (flag)
			{
				flag = await _db.ClaudePromptTemplates.AnyAsync((ClaudePromptTemplate t) => t.Name == name, cancellationToken);
			}
			if (flag)
			{
				throw new ConflictException("A template with the name '" + name + "' already exists.");
			}
			entity.Name = name;
		}
		if (request.Description != null)
		{
			entity.Description = request.Description;
		}
		if (request.Content != null)
		{
			entity.Content = request.Content;
		}
		await _db.SaveChangesAsync(cancellationToken);
		return new ClaudePromptTemplateDto(entity.Id, entity.Name, entity.Description, entity.Content, entity.CreatedDate, entity.ModifiedDate);
	}

	public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default(CancellationToken))
	{
		ClaudePromptTemplate entity = (await _db.ClaudePromptTemplates.FirstOrDefaultAsync((ClaudePromptTemplate t) => t.Id == id, cancellationToken)) ?? throw new NotFoundException("ClaudePromptTemplate", id);
		_db.ClaudePromptTemplates.Remove(entity);
		await _db.SaveChangesAsync(cancellationToken);
	}
}
