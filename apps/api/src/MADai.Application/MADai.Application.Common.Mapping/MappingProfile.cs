using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using MADai.Domain.Tasks;
using MADai.Domain.Workers;
using MADai.Shared.Contracts;

namespace MADai.Application.Common.Mapping;

public class MappingProfile : Profile
{
	public MappingProfile()
	{
		CreateMap<TaskItem, TaskSummaryDto>().ForMember((TaskSummaryDto d) => d.ClaimedByWorkerName, delegate(IMemberConfigurationExpression<TaskItem, TaskSummaryDto, string> o)
		{
			o.MapFrom((TaskItem s) => (s.ClaimedByWorker != null) ? s.ClaimedByWorker.Name : null);
		}).ForMember((TaskSummaryDto d) => d.Queue, delegate(IMemberConfigurationExpression<TaskItem, TaskSummaryDto, string> o)
		{
			o.MapFrom((TaskItem s) => s.QueueName);
		});
		CreateMap<TaskItem, TaskDetailDto>().ForMember((TaskDetailDto d) => d.ClaimedByWorkerName, delegate(IMemberConfigurationExpression<TaskItem, TaskDetailDto, string> o)
		{
			o.MapFrom((TaskItem s) => (s.ClaimedByWorker != null) ? s.ClaimedByWorker.Name : null);
		}).ForMember((TaskDetailDto d) => d.Queue, delegate(IMemberConfigurationExpression<TaskItem, TaskDetailDto, string> o)
		{
			o.MapFrom((TaskItem s) => s.QueueName);
		}).ForMember((TaskDetailDto d) => d.Tags, delegate(IMemberConfigurationExpression<TaskItem, TaskDetailDto, IReadOnlyList<string>> o)
		{
			o.MapFrom((TaskItem s) => (from l in s.TagLinks
				where l.Tag != null
				select l.Tag.Name).ToList());
		})
			.ForMember((TaskDetailDto d) => d.Dependencies, delegate(IMemberConfigurationExpression<TaskItem, TaskDetailDto, IReadOnlyList<TaskDependencyDto>> o)
			{
				o.MapFrom((TaskItem s) => s.Dependencies.Select((TaskDependency x) => new TaskDependencyDto(x.Id, x.DependsOnTaskId, (x.DependsOnTask != null) ? x.DependsOnTask.Title : null, x.DependencyType)).ToList());
			})
			.ForMember((TaskDetailDto d) => d.Artifacts, delegate(IMemberConfigurationExpression<TaskItem, TaskDetailDto, IReadOnlyList<TaskArtifactDto>> o)
			{
				o.MapFrom((TaskItem s) => s.Artifacts.Select((TaskArtifact a) => new TaskArtifactDto(a.Id, a.FileName, a.ContentType, a.SizeBytes, a.StorageProvider, a.PreviewUrl, a.IsFinal, a.CreatedDate, a.Version, a.Kind)).ToList());
			});
		CreateMap<WorkerNode, WorkerHealthDto>().ForMember((WorkerHealthDto d) => d.Status, delegate(IMemberConfigurationExpression<WorkerNode, WorkerHealthDto, string> o)
		{
			o.MapFrom((WorkerNode s) => s.Status.ToString());
		}).ForMember((WorkerHealthDto d) => d.ActiveTasks, delegate(IMemberConfigurationExpression<WorkerNode, WorkerHealthDto, int> o)
		{
			o.MapFrom((WorkerNode s) => s.CurrentConcurrency);
		}).ForMember((WorkerHealthDto d) => d.CpuPercent, delegate(IMemberConfigurationExpression<WorkerNode, WorkerHealthDto, double> o)
		{
			o.MapFrom((WorkerNode s) => (from h in s.Heartbeats
				orderby h.Timestamp descending
				select h.CpuPercent).FirstOrDefault());
		})
			.ForMember((WorkerHealthDto d) => d.MemoryMb, delegate(IMemberConfigurationExpression<WorkerNode, WorkerHealthDto, double> o)
			{
				o.MapFrom((WorkerNode s) => (from h in s.Heartbeats
					orderby h.Timestamp descending
					select h.MemoryMb).FirstOrDefault());
			})
			.ForMember((WorkerHealthDto d) => d.DiskFreeGb, delegate(IMemberConfigurationExpression<WorkerNode, WorkerHealthDto, double> o)
			{
				o.MapFrom((WorkerNode s) => (from h in s.Heartbeats
					orderby h.Timestamp descending
					select h.DiskFreeGb).FirstOrDefault());
			});
	}
}
