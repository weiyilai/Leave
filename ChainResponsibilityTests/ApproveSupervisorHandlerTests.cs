using ChainResponsibility;
using ChainResponsibility.Entities;
using ChainResponsibility.Leave;
using ChainResponsibility.Leave.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ChainResponsibilityTests;

public class ApproveSupervisorHandlerTests
{
    private ServiceProvider _serviceProvider;
    private IApproveSupervisorHandler _handler;

    [SetUp]
    public void Setup()
    {
        // 模擬 DI 容器
        var services = new ServiceCollection();
        services.AddTransient<ApproveManager>();
        services.AddTransient<ApproveDirector>();

        services.AddTransient<IApproveSupervisorHandler>(serviceProvider =>
        {
            var approveManagerHandler = serviceProvider.GetRequiredService<ApproveManager>();

            approveManagerHandler
                .SetNextHandler(serviceProvider.GetRequiredService<ApproveDirector>())
                ;

            return approveManagerHandler;
        });

        _serviceProvider = services.BuildServiceProvider();
        _handler = _serviceProvider.GetRequiredService<IApproveSupervisorHandler>();
    }

    [TearDown]
    public void TearDown()
    {
        _serviceProvider.Dispose();
    }

    [Test]
    public void When_Staff_Application_Annual_Leave_Expected_Approve_Manager()
    {
        // Arrange
        var applicationLeave = new ApplicationLeave
        {
            Employee = new Employee
            {
                Id = "1",
                Name = "Bill",
                OnBoard = new DateTime(2024, 6, 1, 0, 0, 0, DateTimeKind.Utc),
                JobGrade = PositionLevel.Staff,
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
        List<PositionLevel> expected = new List<PositionLevel>()
        {
            PositionLevel.Manager
        };

        // Act
        var actual = _handler.GetApproveSupervisor(applicationLeave);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void When_Staff_Application_Annual_Leave_Expected_Approve_Director()
    {
        // Arrange
        var applicationLeave = new ApplicationLeave
        {
            Employee = new Employee
            {
                Id = "1",
                Name = "Bill",
                OnBoard = new DateTime(2021, 6, 1, 0, 0, 0, DateTimeKind.Utc),
                JobGrade = PositionLevel.Staff,
                LeaveHistory = new List<Leave>
                {
                    new Leave
                    {
                        StartTime = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                        EndTime = new DateTime(2025, 1, 2, 0, 0, 0, DateTimeKind.Utc)
                    }
                }
            },
            Leave = new Leave
            {
                StartTime = new DateTime(2025, 2, 1, 0, 0, 0, DateTimeKind.Utc),
                EndTime = new DateTime(2025, 2, 5, 0, 0, 0, DateTimeKind.Utc),
                PositionLevels = new List<PositionLevel>
                {
                    PositionLevel.Manager,
                    PositionLevel.Director
                }
            }
        };
        List<PositionLevel> expected = new List<PositionLevel>()
        {
            PositionLevel.Manager,
            PositionLevel.Director
        };

        // Act
        var actual = _handler.GetApproveSupervisor(applicationLeave);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void When_Manager_Application_Annual_Leave_Expected_Approve_Director()
    {
        // Arrange
        var applicationLeave = new ApplicationLeave
        {
            Employee = new Employee
            {
                Id = "1",
                Name = "Bill",
                OnBoard = new DateTime(2021, 6, 1, 0, 0, 0, DateTimeKind.Utc),
                JobGrade = PositionLevel.Manager,
                LeaveHistory = new List<Leave>
                {
                    new Leave
                    {
                        StartTime = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                        EndTime = new DateTime(2025, 1, 2, 0, 0, 0, DateTimeKind.Utc)
                    }
                }
            },
            Leave = new Leave
            {
                StartTime = new DateTime(2025, 2, 1, 0, 0, 0, DateTimeKind.Utc),
                EndTime = new DateTime(2025, 2, 1, 0, 0, 0, DateTimeKind.Utc),
                PositionLevels = new List<PositionLevel>
                {
                    PositionLevel.Director
                }
            }
        };
        List<PositionLevel> expected = new List<PositionLevel>()
        {
            PositionLevel.Director
        };

        // Act
        var actual = _handler.GetApproveSupervisor(applicationLeave);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }

    [Test]
    public void When_Manager_Application_Annual_Leave_Expected_Approve_GeneralManager()
    {
        // Arrange
        var applicationLeave = new ApplicationLeave
        {
            Employee = new Employee
            {
                Id = "1",
                Name = "Bill",
                OnBoard = new DateTime(2021, 6, 1, 0, 0, 0, DateTimeKind.Utc),
                JobGrade = PositionLevel.Manager,
                LeaveHistory = new List<Leave>
                {
                    new Leave
                    {
                        StartTime = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                        EndTime = new DateTime(2025, 1, 2, 0, 0, 0, DateTimeKind.Utc)
                    }
                }
            },
            Leave = new Leave
            {
                StartTime = new DateTime(2025, 2, 1, 0, 0, 0, DateTimeKind.Utc),
                EndTime = new DateTime(2025, 2, 7, 0, 0, 0, DateTimeKind.Utc),
                PositionLevels = new List<PositionLevel>
                {
                    PositionLevel.Director
                }
            }
        };
        List<PositionLevel> expected = new List<PositionLevel>()
        {
            PositionLevel.Director,
            PositionLevel.GeneralManager
        };

        // Act
        var actual = _handler.GetApproveSupervisor(applicationLeave);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }
}
