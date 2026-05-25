using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MADai.Application.Abstractions;
using MADai.Application.Common;
using MADai.Domain.Tasks;
using MADai.Shared.Contracts;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MADai.Application.Features.Tasks.Queries;

public class GetTaskByIdQueryHandler : IRequestHandler<GetTaskByIdQuery, TaskDetailDto>
{
	private readonly IDbContextAccess _db;

	private readonly ICurrentUserService _currentUser;

	private readonly IMapper _mapper;

	public GetTaskByIdQueryHandler(IDbContextAccess db, ICurrentUserService currentUser, IMapper mapper)
	{
		_db = db;
		_currentUser = currentUser;
		_mapper = mapper;
	}

	public async Task<TaskDetailDto> Handle(GetTaskByIdQuery request, CancellationToken cancellationToken)
	{
		GetTaskByIdQuery request2 = request;
		IQueryable<TaskItem> query = _db.Tasks.AsNoTracking().Include((TaskItem t) => t.ClaimedByWorker).Include((TaskItem t) => t.Dependencies)
			.ThenInclude((TaskDependency d) => d.DependsOnTask)
			.Include((TaskItem t) => t.Artifacts.Where((TaskArtifact a) => !a.IsDeleted))
			.Include((TaskItem t) => t.TagLinks)
			.ThenInclude((TaskTagLink l) => l.Tag)
			.AsQueryable();
		Guid? companyId2 = _currentUser.CompanyId;
		if (companyId2.HasValue)
		{
			Guid companyId = companyId2.GetValueOrDefault();
			query = query.Where((TaskItem t) => t.CompanyId == companyId);
		}
		TaskItem task = await query.FirstOrDefaultAsync((TaskItem t) => t.Id == request2.Id, cancellationToken);
		if (task == null)
		{
			throw new NotFoundException("Task", request2.Id);
		}
		return _mapper.Map<TaskDetailDto>(task);
	}
}
