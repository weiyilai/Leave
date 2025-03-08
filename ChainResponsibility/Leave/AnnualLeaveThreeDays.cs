using ChainResponsibility.Entities;

namespace ChainResponsibility.Leave;

/// <summary>
/// 六個月以上(含)未滿一年者，三日
/// </summary>
public class AnnualLeaveThreeDays : LeaveHandler
{
    private readonly TimeHelper _timeHelper;

    public AnnualLeaveThreeDays(TimeHelper timeHelper)
    {
        _timeHelper = timeHelper;
    }

    public override int CalculateAllowLeaveDays(ApplicationLeave applicationLeave)
    {
        int onBoardDays = _timeHelper.CalculateTotalDays(applicationLeave.Employee.OnBoard);

        if (onBoardDays >= (int)AnnualLeaveRule.ThreeDays && 
            onBoardDays < (int)AnnualLeaveRule.SevenDays)
        {
            Console.WriteLine($"六個月以上(含)未滿一年者，三日 onBoardDays: {onBoardDays} AccualLeaveDays: 3");
            return 3;
        }

        return base.CalculateAllowLeaveDays(applicationLeave);
    }
}
