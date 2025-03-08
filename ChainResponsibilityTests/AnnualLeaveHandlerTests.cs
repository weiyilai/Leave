using ChainResponsibility;
using ChainResponsibility.Entities;
using ChainResponsibility.Leave;
using ChainResponsibility.Leave.Interfaces;
using ChainResponsibility.Providers.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;

namespace ChainResponsibilityTests;

[TestFixture]
public class AnnualLeaveHandlerTests
{
    private IDateTimeProvider _dateTimeProvider;
    private TimeHelper _timeHelper;
    private ServiceProvider _serviceProvider;
    private ILeaveHandler _handler;
    private DateTime _now;

    [SetUp]
    public void Setup()
    {
        _dateTimeProvider = Substitute.For<IDateTimeProvider>();
        _dateTimeProvider.Now.Returns(new DateTime(2025, 3, 8, 0, 0, 0, DateTimeKind.Utc));
        _timeHelper = new TimeHelper(_dateTimeProvider);

        // 模擬 DI 容器
        var services = new ServiceCollection();
        services.AddSingleton(_dateTimeProvider);
        services.AddTransient(_ => _timeHelper);
        services.AddTransient<NoneAnnualLeave>();
        services.AddTransient<AnnualLeaveThreeDays>();
        services.AddTransient<AnnualLeaveSevenDays>();
        services.AddTransient<AnnualLeaveTenDays>();
        services.AddTransient<AnnualLeaveFourteenDays>();
        services.AddTransient<AnnualLeaveFifteenDays>();
        services.AddTransient<AnnualLeaveSixteenDays>();

        services.AddTransient<ILeaveHandler>(serviceProvider =>
        {
            var noneAnnualleaveHandler = serviceProvider.GetRequiredService<NoneAnnualLeave>();

            noneAnnualleaveHandler
                .SetNextHandler(serviceProvider.GetRequiredService<AnnualLeaveThreeDays>())
                .SetNextHandler(serviceProvider.GetRequiredService<AnnualLeaveSevenDays>())
                .SetNextHandler(serviceProvider.GetRequiredService<AnnualLeaveTenDays>())
                .SetNextHandler(serviceProvider.GetRequiredService<AnnualLeaveFourteenDays>())
                .SetNextHandler(serviceProvider.GetRequiredService<AnnualLeaveFifteenDays>())
                .SetNextHandler(serviceProvider.GetRequiredService<AnnualLeaveSixteenDays>());

            return noneAnnualleaveHandler;
        });

        _serviceProvider = services.BuildServiceProvider();
        _handler = _serviceProvider.GetRequiredService<ILeaveHandler>();

        _now = new DateTime(2025, 3, 8, 0, 0, 0, DateTimeKind.Utc);
    }

    [TearDown]
    public void TearDown()
    {
        _serviceProvider.Dispose();
    }

    /// <summary>
    /// 未滿六個月 不給假
    /// </summary>
    [Test]
    public void When_LessThanSixMonths_Expected_NoneAnnualLeave()
    {
        // Arrange
        var applicationLeave = new ApplicationLeave
        {
            Employee = new Employee
            {
                OnBoard = _now.AddMonths(-6).AddDays(2)
            }
        };
        int expected = 0;

        // Act
        var actual = _handler.CalculateAllowLeaveDays(applicationLeave);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    /// <summary>
    /// 六個月以上(含)未滿一年者，三日
    /// </summary>
    /// <param name="month"></param>
    /// <param name="day"></param>
    [TestCase(-7, 0)]
    [TestCase(-12, 1)]
    public void When_LessThanSixMonths_Expected_AnnualLeaveThreeDays(
        int month,
        int day
        )
    {
        // Arrange
        var applicationLeave = new ApplicationLeave
        {
            Employee = new Employee
            {
                OnBoard = _now.AddMonths(month).AddDays(day)
            }
        };
        int expected = 3;

        // Act
        var actual = _handler.CalculateAllowLeaveDays(applicationLeave);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    /// <summary>
    /// 一年以上(含)未滿二年者，七日
    /// </summary>
    /// <param name="month"></param>
    /// <param name="day"></param>
    [TestCase(-12, 0)]
    [TestCase(-24, 2)]
    public void When_MoreThanOneYear_Inclusive_LessThanTwoYears_Expected_AnnualLeaveSevenDays(
        int month,
        int day
        )
    {
        // Arrange
        var applicationLeave = new ApplicationLeave
        {
            Employee = new Employee
            {
                OnBoard = _now.AddMonths(month).AddDays(day)
            }
        };
        int expected = 7;

        // Act
        var actual = _handler.CalculateAllowLeaveDays(applicationLeave);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    /// <summary>
    /// 二年以上(含)未滿三年者，十日
    /// </summary>
    /// <param name="month"></param>
    /// <param name="day"></param>
    [TestCase(-24, 0)]
    [TestCase(-36, 2)]
    public void When_MoreThanTwoYears_Inclusive_LessThanThreeYears_Expected_AnnualLeaveTenDays(
        int month,
        int day
        )
    {
        // Arrange
        var applicationLeave = new ApplicationLeave
        {
            Employee = new Employee
            {
                OnBoard = _now.AddMonths(month).AddDays(day)
            }
        };
        int expected = 10;

        // Act
        var actual = _handler.CalculateAllowLeaveDays(applicationLeave);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    /// <summary>
    /// 三年以上(含)未滿五年者，每年十四日
    /// </summary>
    /// <param name="month"></param>
    /// <param name="day"></param>
    [TestCase(-36, 0)]
    [TestCase(-60, 2)]
    public void When_MoreThanThreeYears_Inclusive_LessThanFiveYears_Expected_AnnualLeaveFourteenDays(
        int month,
        int day
        )
    {
        // Arrange
        var applicationLeave = new ApplicationLeave
        {
            Employee = new Employee
            {
                OnBoard = _now.AddMonths(month).AddDays(day)
            }
        };
        int expected = 14;

        // Act
        var actual = _handler.CalculateAllowLeaveDays(applicationLeave);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    /// <summary>
    /// 五年以上(含)未滿十年者，每年十五日
    /// </summary>
    /// <param name="month"></param>
    /// <param name="day"></param>
    [TestCase(-60, 0)]
    [TestCase(-120, 4)]
    public void When_MoreThanFiveYears_Inclusive_LessThanTenYears_Expected_AnnualLeaveFifteenDays(
        int month,
        int day
        )
    {
        // Arrange
        var applicationLeave = new ApplicationLeave
        {
            Employee = new Employee
            {
                OnBoard = _now.AddMonths(month).AddDays(day)
            }
        };
        int expected = 15;

        // Act
        var actual = _handler.CalculateAllowLeaveDays(applicationLeave);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    /// <summary>
    /// 十年以上(含)，每滿一年加給一日加至三十日為止
    /// </summary>
    /// <param name="month"></param>
    /// <param name="day"></param>
    [TestCase(-120, 0, 16)]
    [TestCase(-132, 0, 17)]
    [TestCase(-144, 0, 18)]
    [TestCase(-156, 0, 19)]
    [TestCase(-168, 0, 20)]
    [TestCase(-180, 0, 21)]
    [TestCase(-192, 0, 22)]
    [TestCase(-204, 0, 23)]
    [TestCase(-216, 0, 24)]
    [TestCase(-228, 0, 25)]
    [TestCase(-240, 0, 26)]
    [TestCase(-252, 0, 27)]
    [TestCase(-264, 0, 28)]
    [TestCase(-276, 0, 29)]
    [TestCase(-288, 0, 30)]
    [TestCase(-300, 0, 30)]
    [TestCase(-312, 0, 30)]
    [TestCase(-324, 0, 30)]
    [TestCase(-336, 0, 30)]
    [TestCase(-348, 0, 30)]
    public void When_ForTenYearsOrMore_Inclusive_Expected_UpToThe30thDay(
        int month,
        int day,
        int result
        )
    {
        // Arrange
        var applicationLeave = new ApplicationLeave
        {
            Employee = new Employee
            {
                OnBoard = _now.AddMonths(month).AddDays(day)
            }
        };
        int expected = result;

        // Act
        var actual = _handler.CalculateAllowLeaveDays(applicationLeave);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }
}