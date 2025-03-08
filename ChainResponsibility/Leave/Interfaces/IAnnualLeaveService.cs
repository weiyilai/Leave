using ChainResponsibility.Entities;

namespace ChainResponsibility.Leave.Interfaces;

public interface IAnnualLeaveService
{
    /// <summary>
    /// 允許請假
    /// </summary>
    /// <param name="applicationLeave"></param>
    /// <returns>true: 允許 false: 不允許</returns>
    bool CanApproveLeave(ApplicationLeave applicationLeave);
}
