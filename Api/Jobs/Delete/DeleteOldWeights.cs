﻿using Core.Consts;
using Data.Data;
using Microsoft.EntityFrameworkCore;
using Quartz;

namespace Api.Jobs.Delete;

public class DeleteOldWeights : IJob, IScheduled
{
    private static DateOnly Today => DateOnly.FromDateTime(DateTime.UtcNow);

    private readonly CoreContext _coreContext;
    private readonly ILogger<DeleteOldWorkouts> _logger;

    public DeleteOldWeights(ILogger<DeleteOldWorkouts> logger, CoreContext coreContext)
    {
        _logger = logger;
        _coreContext = coreContext;
    }

    public async Task Execute(IJobExecutionContext context)
    {
        try
        {
            await _coreContext.UserVariationWeights.IgnoreQueryFilters()
                .Where(u => u.Date < Today.AddMonths(-1 * UserConsts.DeleteLogsAfterXMonths))
                .ExecuteDeleteAsync();
        }
        catch (Exception e)
        {
            _logger.Log(LogLevel.Error, e, "");
        }
    }

    public static JobKey JobKey => new(nameof(DeleteOldWeights) + "Job", GroupName);
    public static TriggerKey TriggerKey => new(nameof(DeleteOldWeights) + "Trigger", GroupName);
    public static string GroupName => "Delete";

    public static async Task Schedule(IScheduler scheduler)
    {
        var job = JobBuilder.Create<DeleteOldWeights>()
            .WithIdentity(JobKey)
            .Build();

        // Trigger the job every day
        var trigger = TriggerBuilder.Create()
            .WithIdentity(TriggerKey)
            .WithSchedule(CronScheduleBuilder.DailyAtHourAndMinute(0, 0))
            .Build();

        if (await scheduler.GetTrigger(trigger.Key) != null)
        {
            // Update
            await scheduler.RescheduleJob(trigger.Key, trigger);
        }
        else
        {
            // Create
            await scheduler.ScheduleJob(job, trigger);
        }
    }
}
