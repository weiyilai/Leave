using ChainResponsibility.Providers.Interfaces;

namespace ChainResponsibility;

public class TimeHelper
{
    private readonly IDateTimeProvider _dateTimeProvider;

    public TimeHelper(IDateTimeProvider dateTimeProvider)
    {
        _dateTimeProvider = dateTimeProvider;
    }

    public int CalculateTotalDays(DateTime onBoardTime)
    {
        return _dateTimeProvider.Now.Subtract(onBoardTime).Days;
    }
}
