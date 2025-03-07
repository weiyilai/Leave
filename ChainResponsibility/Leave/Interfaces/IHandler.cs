using ChainResponsibility.Entities;

namespace ChainResponsibility.Leave.Interfaces;

public interface IHandler
{
    IHandler SetNextHandler(IHandler nextHandler);
    int CalculateAllowLeaveDays(ApplicationLeave applicationLeave);
}
