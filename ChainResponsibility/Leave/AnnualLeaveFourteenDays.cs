using ChainResponsibility.Entities;

namespace ChainResponsibility.Leave;

/// <summary>
/// 三年以上(含)未滿五年者，每年十四日
/// </summary>
public class AnnualLeaveFourteenDays : LeaveHandler
{
    private readonly TimeHelper _timeHelper;

    public AnnualLeaveFourteenDays(TimeHelper timeHelper)
    {
        _timeHelper = timeHelper;
    }

    public override int CalculateAllowLeaveDays(ApplicationLeave applicationLeave)
    {
        int onBoardDays = _timeHelper.CalculateTotalDays(applicationLeave.Employee.OnBoard);

        if (onBoardDays >= (int)AnnualLeaveRule.FourteenDays && 
            onBoardDays < (int)AnnualLeaveRule.FifteenDays)
        {
            Console.WriteLine($"三年以上(含)未滿五年者，每年十四日 onBoardDays: {onBoardDays} AccualLeaveDays: 14");
            return 14;
        }

        return base.CalculateAllowLeaveDays(applicationLeave);
    }
}
