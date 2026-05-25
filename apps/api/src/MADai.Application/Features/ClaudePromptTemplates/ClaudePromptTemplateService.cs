using MADai.Application.Abstractions;
using MADai.Application.Common;
using MADai.Domain.Tasks;
using MADai.Shared.Contracts;
using Microsoft.EntityFrameworkCore;

namespace MADai.Application.Features.ClaudePromptTemplates;

public interface IClaudePromptTemplateService
{
    Task<IReadOnlyList<ClaudePromptTemplateDto>> ListAsync(CancellationToken cancellationToken = default);
    Task<ClaudePromptTemplateDto> CreateAsync(CreateClaudePromptTemplateRequest request, CancellationToken cancellationToken = default);
    Task<ClaudePromptTemplateDto> UpdateAsync(Guid id, UpdateClaudePromptTemplateRequest request, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
}

public class ClaudePromptTemplateService : IClaudePromptTemplateService
{
    private readonly IDbContextAccess _db;
    public ClaudePromptTemplateService(IDbContextAccess db) => _db = db;

    public async Task<IReadOnlyList<ClaudePromptTemplateDto>> ListAsync(CancellationToken cancellationToken = default)
    {
        return await _db.ClaudePromptTemplates
            .AsNoTracking()
            .OrderBy(t => t.Name)
            .Select(t => new ClaudePromptTemplateDto(t.Id, t.Name, t.Description, t.Content, t.CreatedDate, t.ModifiedDate))
            .ToListAsync(cancellationToken);
    }

    public async Task<ClaudePromptTemplateDto> CreateAsync(CreateClaudePromptTemplateRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Name)) throw new AppException("Template name is required.");
        if (string.IsNullOrWhiteSpace(request.Content)) throw new AppException("Template content is required.");

        var trimmedName = request.Name.Trim();
        if (await _db.ClaudePromptTemplates.AnyAsync(t => t.Name == trimmedName, cancellationToken))
        {
            throw new ConflictException($"A template with the name '{trimmedName}' already exists.");
        }

        var entity = new ClaudePromptTemplate
        {
            Name = trimmedName,
            Description = request.Description,
            Content = request.Content
        };
        _db.ClaudePromptTemplates.Add(entity);
        await _db.SaveChangesAsync(cancellationToken);
        return new ClaudePromptTemplateDto(entity.Id, entity.Name, entity.Description, entity.Content, entity.CreatedDate, entity.ModifiedDate);
    }

    public async Task<ClaudePromptTemplateDto> UpdateAsync(Guid id, UpdateClaudePromptTemplateRequest request, CancellationToken cancellationToken = default)
    {
        var entity = await _db.ClaudePromptTemplates.FirstOrDefaultAsync(t => t.Id == id, cancellationToken)
            ?? throw new NotFoundException("ClaudePromptTemplate", id);

        if (request.Name is not null && request.Name.Length > 0)
        {
            var name = request.Name.Trim();
            if (name != entity.Name && await _db.ClaudePromptTemplates.AnyAsync(t => t.Name == name, cancellationToken))
            {
                throw new ConflictException($"A template with the name '{name}' already exists.");
            }
            entity.Name = name;
        }
        if (request.Description is not null) entity.Description = request.Description;
        if (request.Content is not null) entity.Content = request.Content;

        await _db.SaveChangesAsync(cancellationToken);
        return new ClaudePromptTemplateDto(entity.Id, entity.Name, entity.Description, entity.Content, entity.CreatedDate, entity.ModifiedDate);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _db.ClaudePromptTemplates.FirstOrDefaultAsync(t => t.Id == id, cancellationToken)
            ?? throw new NotFoundException("ClaudePromptTemplate", id);
        _db.ClaudePromptTemplates.Remove(entity);
        await _db.SaveChangesAsync(cancellationToken);
    }
}
