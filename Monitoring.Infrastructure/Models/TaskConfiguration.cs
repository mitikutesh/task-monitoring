using System;
using System.Collections.Generic;

namespace Monitoring.Infrastructure.Models
{
    public record Interval(int Minutes, int Hours, int Days);
    public record RunOn(string Day, string From, string To);
    public record TasksToDo(Guid Id, string Type, Interval Interval, string Display, List<RunOn> RunOns, string HostName, string ConnectionString, string Query);
    public record TaskService(Interval PollingFrequency, List<RunOn> RunOns);

    public record TaskConfiguration(string Id, string Name, TaskService Service, List<TasksToDo> Tasks);
}