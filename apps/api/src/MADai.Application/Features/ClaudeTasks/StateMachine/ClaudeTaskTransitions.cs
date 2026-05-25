using MADai.Domain.Enums;

namespace MADai.Application.Features.ClaudeTasks.StateMachine;

/// <summary>
/// Single source of truth for legal ClaudeTask status transitions. Both the worker's
/// PATCH path and the operator's PATCH path go through this gate.
/// </summary>
public static class ClaudeTaskTransitions
{
    private static readonly IReadOnlyDictionary<ClaudeTaskStatus, IReadOnlySet<ClaudeTaskStatus>> _allowed =
        new Dictionary<ClaudeTaskStatus, IReadOnlySet<ClaudeTaskStatus>>
        {
            [ClaudeTaskStatus.Pending] = new HashSet<ClaudeTaskStatus>
            {
                ClaudeTaskStatus.InProgress, ClaudeTaskStatus.Cancelled
            },
            [ClaudeTaskStatus.InProgress] = new HashSet<ClaudeTaskStatus>
            {
                ClaudeTaskStatus.Completed, ClaudeTaskStatus.ToBeDeployed,
                ClaudeTaskStatus.Failed, ClaudeTaskStatus.Deferred, ClaudeTaskStatus.Pending
            },
            [ClaudeTaskStatus.ToBeDeployed] = new HashSet<ClaudeTaskStatus>
            {
                ClaudeTaskStatus.Completed, ClaudeTaskStatus.Failed
            },
            [ClaudeTaskStatus.Deferred] = new HashSet<ClaudeTaskStatus>
            {
                ClaudeTaskStatus.Pending, ClaudeTaskStatus.Cancelled
            },
            // Terminal statuses - changes require ?override=true.
            [ClaudeTaskStatus.Completed] = new HashSet<ClaudeTaskStatus>(),
            [ClaudeTaskStatus.Cancelled] = new HashSet<ClaudeTaskStatus>(),
            [ClaudeTaskStatus.Failed]    = new HashSet<ClaudeTaskStatus>()
        };

    public static bool IsTerminal(ClaudeTaskStatus s) =>
        s is ClaudeTaskStatus.Completed or ClaudeTaskStatus.Cancelled or ClaudeTaskStatus.Failed;

    public static bool IsAllowed(ClaudeTaskStatus from, ClaudeTaskStatus to)
    {
        if (from == to) return true; // no-op is always fine
        return _allowed.TryGetValue(from, out var set) && set.Contains(to);
    }
}
