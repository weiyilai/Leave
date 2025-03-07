﻿using ChainResponsibility.Entities;

namespace ChainResponsibility.Leave;

/// <summary>
/// 二年以上(含)未滿三年者，十日
/// </summary>
public class AnnualLeaveTenDays : LeaveHandler
{
    private TimeHelper _timeHelper;

    public AnnualLeaveTenDays(TimeHelper timeHelper)
    {
        _timeHelper = timeHelper;
    }

    public override int CalculateAllowLeaveDays(ApplicationLeave applicationLeave)
    {
        int onBoardDays = _timeHelper.CalculateTotalDays(applicationLeave.Employee.OnBoard);

        Console.WriteLine($"AnnualLeaveTenDays onBoardDays: {onBoardDays}");

        if (onBoardDays >= (int)AnnualLeaveRule.TenDays &&
            onBoardDays < (int)AnnualLeaveRule.FourteenDays)
        {
            return 10;
        }

        return base.CalculateAllowLeaveDays(applicationLeave);
    }
}
