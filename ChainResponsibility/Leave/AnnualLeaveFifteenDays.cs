using ChainResponsibility.Entities;

namespace ChainResponsibility.Leave;

/// <summary>
/// 五年以上(含)未滿十年者，每年十五日
/// </summary>
public class AnnualLeaveFifteenDays : LeaveHandler
{
    private readonly TimeHelper _timeHelper;

    public AnnualLeaveFifteenDays(TimeHelper timeHelper)
    {
        _timeHelper = timeHelper;
    }

    public override int CalculateAllowLeaveDays(ApplicationLeave applicationLeave)
    {
        int onBoardDays = _timeHelper.CalculateTotalDays(applicationLeave.Employee.OnBoard);

        if (onBoardDays >= (int)AnnualLeaveRule.FifteenDays &&
            onBoardDays < (int)AnnualLeaveRule.SixteenDays)
        {
            Console.WriteLine($"五年以上(含)未滿十年者，每年十五日 onBoardDays: {onBoardDays} AccualLeaveDays: 15");
            return 15;
        }

        return base.CalculateAllowLeaveDays(applicationLeave);
    }
}
