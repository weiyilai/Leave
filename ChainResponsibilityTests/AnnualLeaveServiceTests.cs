using ChainResponsibility.Providers.Interfaces;
using ChainResponsibility;
using NSubstitute;
using ChainResponsibility.Leave.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using ChainResponsibility.Leave;
using ChainResponsibility.Entities;
using System;

namespace ChainResponsibilityTests;

[TestFixture]
public class AnnualLeaveServiceTests
{
    private ILeaveHandler _handler;
    private AnnualLeaveService _annualLeaveService;

    [SetUp]
    public void Setup()
    {
        // 使用 NSubstitute 創建假物件
        _handler = Substitute.For<ILeaveHandler>();

        // 初始化被測試的服務，注入假物件
        _annualLeaveService = new AnnualLeaveService(_handler);
    }

    [Test]
    public void When_LessThanSixMonths_Expected_False()
    {
        // Arrange
        _handler.CalculateAllowLeaveDays(Arg.Any<ApplicationLeave>()).Returns(0);
        var applicationLeave = new ApplicationLeave
        {
            Employee = new Employee
            {
                Id = "1",
                Name = "Bruce",
                OnBoard = new DateTime(2024, 6, 1, 0, 0, 0, DateTimeKind.Utc),
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
                EndTime = new DateTime(2025, 2, 2, 0, 0, 0, DateTimeKind.Utc)
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
        _handler.CalculateAllowLeaveDays(Arg.Any<ApplicationLeave>()).Returns(3);
        var applicationLeave = new ApplicationLeave
        {
            Employee = new Employee
            {
                Id = "1",
                Name = "Bruce",
                OnBoard = new DateTime(2024, 6, 1, 0, 0, 0, DateTimeKind.Utc),
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
                EndTime = new DateTime(2025, 2, 3, 0, 0, 0, DateTimeKind.Utc)
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
        _handler.CalculateAllowLeaveDays(Arg.Any<ApplicationLeave>()).Returns(3);
        var applicationLeave = new ApplicationLeave
        {
            Employee = new Employee
            {
                Id = "1",
                Name = "Bruce",
                OnBoard = new DateTime(2024, 6, 1, 0, 0, 0, DateTimeKind.Utc),
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
                EndTime = new DateTime(2025, 2, 2, 0, 0, 0, DateTimeKind.Utc)
            }
        };
        bool expected = true;

        // Act
        var actual = _annualLeaveService.CanApproveLeave(applicationLeave);

        // Assert
        Assert.That(actual, Is.EqualTo(expected));
    }
}
