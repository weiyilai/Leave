using ChainResponsibility;
using ChainResponsibility.Entities;
using ChainResponsibility.Leave;
using ChainResponsibility.Leave.Interfaces;
using NSubstitute;

namespace ChainResponsibilityTests;

[TestFixture]
public class AnnualLeaveServiceTests
{
    private ILeaveHandler _leaveHandler;
    private AnnualLeaveService _annualLeaveService;
    private IApproveSupervisorHandler _approveSupervisorHandler;

    [SetUp]
    public void Setup()
    {
        // 使用 NSubstitute 創建假物件
        _leaveHandler = Substitute.For<ILeaveHandler>();
        _approveSupervisorHandler = Substitute.For<IApproveSupervisorHandler>();

        // 初始化被測試的服務，注入假物件
        _annualLeaveService = 
            new AnnualLeaveService(
                _leaveHandler, 
                _approveSupervisorHandler
                );
    }

    [Test]
    public void When_LessThanSixMonths_Expected_False()
    {
        // Arrange
        _leaveHandler.CalculateAllowLeaveDays(Arg.Any<ApplicationLeave>()).Returns(0);
        _approveSupervisorHandler.GetApproveSupervisor(Arg.Any<ApplicationLeave>()).Returns(new List<PositionLevel> { PositionLevel.Manager });

        var applicationLeave = new ApplicationLeave
        {
            Employee = new Employee
            {
                Id = "1",
                Name = "Bruce",
                OnBoard = new DateTime(2024, 6, 1, 0, 0, 0, DateTimeKind.Utc),
                JobGrade = PositionLevel.Manager,
                LeaveHistory = new List<Leave>
                {
                    new Leave
                    {
                        StartTime = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                        EndTime = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                    }
                }
            },
            Leave = new Leave
            {
                StartTime = new DateTime(2025, 2, 1, 0, 0, 0, DateTimeKind.Utc),
                EndTime = new DateTime(2025, 2, 2, 0, 0, 0, DateTimeKind.Utc),
                PositionLevels = new List<PositionLevel>
                {
                    PositionLevel.Manager
                }
            }
        };
        bool expected = false;

        // Act
        var actual = _annualLeaveService.CanApproveLeave(applicationLeave);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void When_MoreThanSixMonths_Expected_False()
    {
        // Arrange
        _leaveHandler.CalculateAllowLeaveDays(Arg.Any<ApplicationLeave>()).Returns(3);
        _approveSupervisorHandler.GetApproveSupervisor(Arg.Any<ApplicationLeave>()).Returns(new List<PositionLevel> { PositionLevel.Manager });

        var applicationLeave = new ApplicationLeave
        {
            Employee = new Employee
            {
                Id = "1",
                Name = "Bruce",
                OnBoard = new DateTime(2024, 6, 1, 0, 0, 0, DateTimeKind.Utc),
                JobGrade = PositionLevel.Manager,
                LeaveHistory = new List<Leave>
                {
                    new Leave
                    {
                        StartTime = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                        EndTime = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                    }
                }
            },
            Leave = new Leave
            {
                StartTime = new DateTime(2025, 2, 1, 0, 0, 0, DateTimeKind.Utc),
                EndTime = new DateTime(2025, 2, 3, 0, 0, 0, DateTimeKind.Utc),
                PositionLevels = new List<PositionLevel>
                {
                    PositionLevel.Manager
                }
            }
        };
        bool expected = false;

        // Act
        var actual = _annualLeaveService.CanApproveLeave(applicationLeave);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void When_MoreThanSixMonths_Expected_True()
    {
        // Arrange
        _leaveHandler.CalculateAllowLeaveDays(Arg.Any<ApplicationLeave>()).Returns(3);
        _approveSupervisorHandler.GetApproveSupervisor(Arg.Any<ApplicationLeave>()).Returns(new List<PositionLevel> { PositionLevel.Manager });

        var applicationLeave = new ApplicationLeave
        {
            Employee = new Employee
            {
                Id = "1",
                Name = "Bruce",
                OnBoard = new DateTime(2024, 6, 1, 0, 0, 0, DateTimeKind.Utc),
                JobGrade = PositionLevel.Manager,
                LeaveHistory = new List<Leave>
                {
                    new Leave
                    {
                        StartTime = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                        EndTime = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                    }
                }
            },
            Leave = new Leave
            {
                StartTime = new DateTime(2025, 2, 1, 0, 0, 0, DateTimeKind.Utc),
                EndTime = new DateTime(2025, 2, 2, 0, 0, 0, DateTimeKind.Utc),
                PositionLevels = new List<PositionLevel>
                {
                    PositionLevel.Manager
                }
            }
        };
        bool expected = true;

        // Act
        var actual = _annualLeaveService.CanApproveLeave(applicationLeave);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }
}
