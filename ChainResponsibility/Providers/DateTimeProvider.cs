using ChainResponsibility.Providers.Interfaces;

namespace ChainResponsibility.Providers;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime Now => DateTime.Now;
}
