using ChainResponsibility.Entities;

namespace ChainResponsibility.Leave.Interfaces;

public interface IApproveSupervisorHandler
{
    List<PositionLevel> GetApproveSupervisor(ApplicationLeave applicationLeave);
    IApproveSupervisorHandler SetNextHandler(IApproveSupervisorHandler nextHandler);
}
