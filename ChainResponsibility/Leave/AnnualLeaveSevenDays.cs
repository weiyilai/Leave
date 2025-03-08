using ChainResponsibility.Entities;

namespace ChainResponsibility.Leave;

/// <summary>
/// 一年以上(含)未滿二年者，七日
/// </summary>
public class AnnualLeaveSevenDays : LeaveHandler
{
    private readonly TimeHelper _timeHelper;

    public AnnualLeaveSevenDays(TimeHelper timeHelper)
    {
        _timeHelper = timeHelper;
    }

    public override int CalculateAllowLeaveDays(ApplicationLeave applicationLeave)
    {
        int onBoardDays = _timeHelper.CalculateTotalDays(applicationLeave.Employee.OnBoard);

        if (onBoardDays >= (int)AnnualLeaveRule.SevenDays && 
            onBoardDays < (int)AnnualLeaveRule.TenDays)
        {
            Console.WriteLine($"一年以上(含)未滿二年者，七日 onBoardDays: {onBoardDays} AccualLeaveDays: 7");
            return 7;
        }

        return base.CalculateAllowLeaveDays(applicationLeave);
    }
}
