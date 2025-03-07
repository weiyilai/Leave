using ChainResponsibility.Entities;
using ChainResponsibility.Leave.Interfaces;

namespace ChainResponsibility.Leave;

public abstract class LeaveHandler : IHandler
{
    private IHandler? _nextHandler;

    public IHandler SetNextHandler(IHandler nextHandler)
    {
        _nextHandler = nextHandler;
        return nextHandler;
    }

    public virtual int CalculateAllowLeaveDays(ApplicationLeave applicationLeave)
    {
        return _nextHandler?.CalculateAllowLeaveDays(applicationLeave) ?? 0;
    }
}
