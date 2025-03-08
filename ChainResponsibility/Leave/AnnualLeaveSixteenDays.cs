using ChainResponsibility.Entities;

namespace ChainResponsibility.Leave;

/// <summary>
/// 十年以上(含)，每滿一年加給一日加至三十日為止
/// </summary>
public class AnnualLeaveSixteenDays : LeaveHandler
{
    private readonly TimeHelper _timeHelper;

    public AnnualLeaveSixteenDays(TimeHelper timeHelper)
    {
        _timeHelper = timeHelper;
    }

    public override int CalculateAllowLeaveDays(ApplicationLeave applicationLeave)
    {
        int onBoardDays = _timeHelper.CalculateTotalDays(applicationLeave.Employee.OnBoard);

        if (onBoardDays >= (int)AnnualLeaveRule.SixteenDays)
        {
            int calAccualLeaveDays = onBoardDays / 365 - 10 + 16;
            int result = calAccualLeaveDays <= 30 ? calAccualLeaveDays : 30;

            Console.WriteLine($"十年以上(含)，每滿一年加給一日加至三十日為止 onBoardDays: {onBoardDays} AccualLeaveDays: {result}");

            return result;
        }

        return base.CalculateAllowLeaveDays(applicationLeave);
    }
}
