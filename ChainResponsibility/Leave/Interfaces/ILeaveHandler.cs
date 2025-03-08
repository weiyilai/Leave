using ChainResponsibility.Entities;

namespace ChainResponsibility.Leave.Interfaces;

public interface ILeaveHandler
{
    ILeaveHandler SetNextHandler(ILeaveHandler nextHandler);
    int CalculateAllowLeaveDays(ApplicationLeave applicationLeave);
}
