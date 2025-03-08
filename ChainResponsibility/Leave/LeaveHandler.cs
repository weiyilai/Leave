using ChainResponsibility.Entities;
using ChainResponsibility.Leave.Interfaces;

namespace ChainResponsibility.Leave;

public abstract class LeaveHandler : ILeaveHandler
{
    private ILeaveHandler? _nextHandler;

    public ILeaveHandler SetNextHandler(ILeaveHandler nextHandler)
    {
        _nextHandler = nextHandler;
        return nextHandler;
    }

    public virtual int CalculateAllowLeaveDays(ApplicationLeave applicationLeave)
    {
        return _nextHandler?.CalculateAllowLeaveDays(applicationLeave) ?? 0;
    }
}
