using ChainResponsibility.Entities;
using ChainResponsibility.Leave.Interfaces;

namespace ChainResponsibility.Leave;

public class AnnualLeaveService : IAnnualLeaveService
{
    private readonly ILeaveHandler _leaveHandler;

    public AnnualLeaveService(ILeaveHandler leaveHandler)
    {
        _leaveHandler = leaveHandler;
    }

    public bool CanApproveLeave(ApplicationLeave applicationLeave)
    {
        int annualLeaveDays = _leaveHandler.CalculateAllowLeaveDays(applicationLeave);
        int historyDays = applicationLeave.Employee.LeaveHistory.Sum(x => (x.EndTime - x.StartTime).Days) + 1;
        int applicationDays = (applicationLeave.Leave.EndTime - applicationLeave.Leave.StartTime).Days + 1;
        bool canApprove = historyDays + applicationDays <= annualLeaveDays;

        Console.WriteLine($"historyDays: {historyDays} applicationDays: {applicationDays} annualLeaveDays: {annualLeaveDays} canApprove: {canApprove}");

        return canApprove;
    }
}
