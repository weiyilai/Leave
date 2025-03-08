using ChainResponsibility.Entities;

namespace ChainResponsibility.Leave;

/// <summary>
/// 未滿六個月 不給假
/// </summary>
public class NoneAnnualLeave : LeaveHandler
{
    private TimeHelper _timeHelper;

    public NoneAnnualLeave(TimeHelper timeHelper)
    {
        _timeHelper = timeHelper;
    }

    public override int CalculateAllowLeaveDays(ApplicationLeave applicationLeave)
    {
        int onBoardDays = _timeHelper.CalculateTotalDays(applicationLeave.Employee.OnBoard);

        if (onBoardDays < (int)AnnualLeaveRule.ThreeDays)
        {
            Console.WriteLine($"未滿六個月 不給假 onBoardDays: {onBoardDays} AnnualLeaveDays: 0");
            return 0;
        }

        return base.CalculateAllowLeaveDays(applicationLeave);
    }
}
