using ChainResponsibility.Entities;
using ChainResponsibility.Leave.Interfaces;

namespace ChainResponsibility.Leave;

public abstract class ApproveSupervisorHandler : IApproveSupervisorHandler
{
    private IApproveSupervisorHandler? _nextHandler;

    public virtual List<PositionLevel> GetApproveSupervisor(ApplicationLeave applicationLeave)
    {
        return _nextHandler?.GetApproveSupervisor(applicationLeave) ?? new List<PositionLevel>()
        {
            PositionLevel.Manager
        };
    }

    public IApproveSupervisorHandler SetNextHandler(IApproveSupervisorHandler nextHandler)
    {
        _nextHandler = nextHandler;
        return nextHandler;
    }
}
