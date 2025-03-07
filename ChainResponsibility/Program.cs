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

// 註冊一個方法來構建責任鏈
builder.Services.AddTransient<IHandler>(serviceProvider =>
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

using IHost host = builder.Build();

var handler = host.Services.GetRequiredService<IHandler>();

Console.WriteLine(handler.CalculateAllowLeaveDays(new ApplicationLeave
{
    Employee = new Employee
    {
        OnBoard = new DateTime(2019, 1, 1)
    },
    Leave = new Leave
    {
        StartTime = new DateTime(2021, 1, 1),
        EndTime = new DateTime(2021, 1, 10)
    }
}));

await host.RunAsync();
