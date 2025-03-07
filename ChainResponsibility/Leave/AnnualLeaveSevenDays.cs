using ChainResponsibility.Entities;

namespace ChainResponsibility.Leave;

/// <summary>
/// 一年以上(含)未滿二年者，七日
/// </summary>
public class AnnualLeaveSevenDays : LeaveHandler
{
    private TimeHelper _timeHelper;

    public AnnualLeaveSevenDays(TimeHelper timeHelper)
    {
        _timeHelper = timeHelper;
    }

    public override int CalculateAllowLeaveDays(ApplicationLeave applicationLeave)
    {
        int onBoardDays = _timeHelper.CalculateTotalDays(applicationLeave.Employee.OnBoard);

        Console.WriteLine($"AnnualLeaveSevenDays onBoardDays: {onBoardDays}");

        if (onBoardDays >= (int)AnnualLeaveRule.SevenDays && 
            onBoardDays < (int)AnnualLeaveRule.TenDays)
        {
            return 7;
        }

        return base.CalculateAllowLeaveDays(applicationLeave);
    }
}
