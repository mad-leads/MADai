using System.Collections.Generic;
using MADai.Domain.Enums;

namespace MADai.Application.Features.ClaudeTasks.StateMachine;

public static class ClaudeTaskTransitions
{
	private static readonly IReadOnlyDictionary<ClaudeTaskStatus, IReadOnlySet<ClaudeTaskStatus>> _allowed = new Dictionary<ClaudeTaskStatus, IReadOnlySet<ClaudeTaskStatus>>
	{
		[ClaudeTaskStatus.Pending] = new HashSet<ClaudeTaskStatus>
		{
			ClaudeTaskStatus.InProgress,
			ClaudeTaskStatus.Cancelled
		},
		[ClaudeTaskStatus.InProgress] = new HashSet<ClaudeTaskStatus>
		{
			ClaudeTaskStatus.Completed,
			ClaudeTaskStatus.ToBeDeployed,
			ClaudeTaskStatus.Failed,
			ClaudeTaskStatus.Deferred,
			ClaudeTaskStatus.Pending
		},
		[ClaudeTaskStatus.ToBeDeployed] = new HashSet<ClaudeTaskStatus>
		{
			ClaudeTaskStatus.Completed,
			ClaudeTaskStatus.Failed
		},
		[ClaudeTaskStatus.Deferred] = new HashSet<ClaudeTaskStatus>
		{
			ClaudeTaskStatus.Pending,
			ClaudeTaskStatus.Cancelled
		},
		[ClaudeTaskStatus.Completed] = new HashSet<ClaudeTaskStatus>(),
		[ClaudeTaskStatus.Cancelled] = new HashSet<ClaudeTaskStatus>(),
		[ClaudeTaskStatus.Failed] = new HashSet<ClaudeTaskStatus>()
	};

	public static bool IsTerminal(ClaudeTaskStatus s)
	{
		if (s == ClaudeTaskStatus.Completed || s == ClaudeTaskStatus.Failed || s == ClaudeTaskStatus.Cancelled)
		{
			return true;
		}
		return false;
	}

	public static bool IsAllowed(ClaudeTaskStatus from, ClaudeTaskStatus to)
	{
		if (from == to)
		{
			return true;
		}
		if (_allowed.TryGetValue(from, out IReadOnlySet<ClaudeTaskStatus> set))
		{
			return set.Contains(to);
		}
		return false;
	}
}
