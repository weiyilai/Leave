using ChainResponsibility.Entities;
using ChainResponsibility.Leave.Interfaces;

namespace ChainResponsibility.Leave;

public class AnnualLeaveService : IAnnualLeaveService
{
    private readonly ILeaveHandler _leaveHandler;
    private readonly IApproveSupervisorHandler _approveSupervisorHandler;

    public AnnualLeaveService(
        ILeaveHandler leaveHandler,
        IApproveSupervisorHandler approveSupervisorHandler
        )
    {
        _leaveHandler = leaveHandler;
        _approveSupervisorHandler = approveSupervisorHandler;
    }

    public bool CanApproveLeave(ApplicationLeave applicationLeave)
    {
        int annualLeaveDays = _leaveHandler.CalculateAllowLeaveDays(applicationLeave);
        int historyDays = applicationLeave.Employee.LeaveHistory.Sum(x => (x.EndTime - x.StartTime).Days) + 1;
        int applicationDays = (applicationLeave.Leave.EndTime - applicationLeave.Leave.StartTime).Days + 1;
        var approveSupervisors = _approveSupervisorHandler.GetApproveSupervisor(applicationLeave);
        bool isExistsApproveSupervisor = applicationLeave.Leave.PositionLevels.SequenceEqual(approveSupervisors);
        bool canApprove = historyDays + applicationDays <= annualLeaveDays && isExistsApproveSupervisor;

        Console.WriteLine($"historyDays: {historyDays} applicationDays: {applicationDays} annualLeaveDays: {annualLeaveDays} canApprove: {canApprove}");

        return canApprove;
    }
}
