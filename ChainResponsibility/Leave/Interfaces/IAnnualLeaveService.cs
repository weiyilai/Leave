using ChainResponsibility.Entities;

namespace ChainResponsibility.Leave.Interfaces;

public interface IAnnualLeaveService
{
    bool CanApproveLeave(ApplicationLeave applicationLeave);
}
