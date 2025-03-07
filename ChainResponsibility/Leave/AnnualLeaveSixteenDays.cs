using ChainResponsibility.Entities;

namespace ChainResponsibility.Leave;

/// <summary>
/// 十年以上(含)，每滿一年加給一日加至三十日為止
/// </summary>
public class AnnualLeaveSixteenDays : LeaveHandler
{
    private TimeHelper _timeHelper;

    public AnnualLeaveSixteenDays(TimeHelper timeHelper)
    {
        _timeHelper = timeHelper;
    }

    public override int CalculateAllowLeaveDays(ApplicationLeave applicationLeave)
    {
        int onBoardDays = _timeHelper.CalculateTotalDays(applicationLeave.Employee.OnBoard);

        Console.WriteLine($"AnnualLeaveSixteenDays onBoardDays: {onBoardDays}");

        if (onBoardDays >= (int)AnnualLeaveRule.SixteenDays)
        {
            int accualDays = onBoardDays / 365 - 10 + 16;
            return accualDays <= 30 ? accualDays :　30;
        }

        return base.CalculateAllowLeaveDays(applicationLeave);
    }
}
