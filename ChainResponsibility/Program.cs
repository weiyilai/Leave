// See https://aka.ms/new-console-template for more information
using ChainResponsibility;
using ChainResponsibility.Entities;
using ChainResponsibility.Leave;
using ChainResponsibility.Providers.Interfaces;
using ChainResponsibility.Providers;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ChainResponsibility.Leave.Interfaces;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
builder.Services.AddTransient<TimeHelper>();
builder.Services.AddTransient<NoneAnnualLeave>();
builder.Services.AddTransient<AnnualLeaveThreeDays>();
builder.Services.AddTransient<AnnualLeaveSevenDays>();
builder.Services.AddTransient<AnnualLeaveTenDays>();
builder.Services.AddTransient<AnnualLeaveFourteenDays>();
builder.Services.AddTransient<AnnualLeaveFifteenDays>();
builder.Services.AddTransient<AnnualLeaveSixteenDays>();
builder.Services.AddTransient<IAnnualLeaveService, AnnualLeaveService>();

builder.Services.AddTransient<ApproveManager>();
builder.Services.AddTransient<ApproveDirector>();

// 註冊一個方法來構建責任鏈
builder.Services.AddTransient<ILeaveHandler>(serviceProvider =>
{
    var noneAnnualleaveHandler = serviceProvider.GetRequiredService<NoneAnnualLeave>();

    noneAnnualleaveHandler
        .SetNextHandler(serviceProvider.GetRequiredService<AnnualLeaveThreeDays>())
        .SetNextHandler(serviceProvider.GetRequiredService<AnnualLeaveSevenDays>())
        .SetNextHandler(serviceProvider.GetRequiredService<AnnualLeaveTenDays>())
        .SetNextHandler(serviceProvider.GetRequiredService<AnnualLeaveFourteenDays>())
        .SetNextHandler(serviceProvider.GetRequiredService<AnnualLeaveFifteenDays>())
        .SetNextHandler(serviceProvider.GetRequiredService<AnnualLeaveSixteenDays>())
        ;

    return noneAnnualleaveHandler;
});

builder.Services.AddTransient<IApproveSupervisorHandler>(serviceProvider =>
{
    var approveManagerHandler = serviceProvider.GetRequiredService<ApproveManager>();

    approveManagerHandler
        .SetNextHandler(serviceProvider.GetRequiredService<ApproveDirector>())
        ;

    return approveManagerHandler;
});

using IHost host = builder.Build();

var annualLeaveService = host.Services.GetRequiredService<IAnnualLeaveService>();

var applicationLeaveByBrian = new ApplicationLeave
{
    Employee = new Employee
    {
        Id = "1",
        Name = "Brian",
        OnBoard = new DateTime(2019, 1, 1, 0, 0, 0, DateTimeKind.Utc),
        JobGrade = PositionLevel.Manager,
        LeaveHistory = new List<Leave>
        {
            new Leave
            {
                StartTime = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                EndTime = new DateTime(2020, 1, 5, 0, 0, 0, DateTimeKind.Utc)
            },
            new Leave
            {
                StartTime = new DateTime(2020, 2, 1, 0, 0, 0, DateTimeKind.Utc),
                EndTime = new DateTime(2020, 2, 5, 0, 0, 0, DateTimeKind.Utc)
            }
        }
    },
    Leave = new Leave
    {
        StartTime = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc),
        EndTime = new DateTime(2023, 1, 2, 0, 0, 0, DateTimeKind.Utc),
        PositionLevels = new List<PositionLevel>
        {
            PositionLevel.Director
        }
    }
};

annualLeaveService.CanApproveLeave(
    applicationLeaveByBrian
    );

var applicationLeaveByJeff = new ApplicationLeave
{
    Employee = new Employee
    {
        Id = "2",
        Name = "Jeff",
        OnBoard = new DateTime(2019, 1, 1, 0, 0, 0, DateTimeKind.Utc),
        JobGrade = PositionLevel.Manager,
        LeaveHistory = new List<Leave>
        {
            new Leave
            {
                StartTime = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                EndTime = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new Leave
            {
                StartTime = new DateTime(2020, 2, 1, 0, 0, 0, DateTimeKind.Utc),
                EndTime = new DateTime(2020, 2, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        }
    },
    Leave = new Leave
    {
        StartTime = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc),
        EndTime = new DateTime(2023, 1, 10, 0, 0, 0, DateTimeKind.Utc),
        PositionLevels = new List<PositionLevel>
        {
            PositionLevel.Director,
            PositionLevel.GeneralManager
        }
    }
};

annualLeaveService.CanApproveLeave(
    applicationLeaveByJeff
    );

var applicationLeaveByBruce = new ApplicationLeave
{
    Employee = new Employee
    {
        Id = "11",
        Name = "Bruce",
        OnBoard = new DateTime(2020, 6, 1, 0, 0, 0, DateTimeKind.Utc),
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
        EndTime = new DateTime(2025, 2, 6, 0, 0, 0, DateTimeKind.Utc),
        PositionLevels = new List<PositionLevel>
        {
            PositionLevel.Manager,
            PositionLevel.Director
        }
    }
};

annualLeaveService.CanApproveLeave(
    applicationLeaveByBruce
    );

var applicationLeaveByBill = new ApplicationLeave
{
    Employee = new Employee
    {
        Id = "12",
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

annualLeaveService.CanApproveLeave(
    applicationLeaveByBill
    );

await host.RunAsync();
