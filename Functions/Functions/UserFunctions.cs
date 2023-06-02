using FinerFettle.Functions.Data;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FinerFettle.Functions.Functions;

public class UserFunctions
{
    private readonly CoreContext _coreContext;

    public UserFunctions(CoreContext coreContext)
    {
        _coreContext = coreContext;
    }

    [Function(nameof(DisableInactiveUsers))]
    public async Task DisableInactiveUsers([TimerTrigger(/*Weekly*/ "0 0 0 * * 0", RunOnStartup = Core.Debug.Consts.IsDebug)] TimerInfo timerInfo)
    {
        const string disabledReason = "No recent account activity";

        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var inactiveUsers = await _coreContext.Users
            .Where(u => u.DisabledReason == null)
            // User has no account activity in the past X months
            .Where(u => (u.LastActive.HasValue && u.LastActive.Value < today.AddMonths(-1 * Core.User.Consts.DisableAfterXMonths))
                || (!u.LastActive.HasValue && u.CreatedDate < today.AddMonths(-1 * Core.User.Consts.DisableAfterXMonths))
            )
            .ToListAsync();

        foreach (var user in inactiveUsers)
        {
            user.DisabledReason = disabledReason;
        }

        await _coreContext.SaveChangesAsync();
    }

    [Function(nameof(DeleteInactiveUsers))]
    public async Task DeleteInactiveUsers([TimerTrigger(/*Monthly*/ "0 0 0 1 * *", RunOnStartup = Core.Debug.Consts.IsDebug)] TimerInfo timerInfo)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var toDeleteUsers = await _coreContext.Users
            // User is disabled
            .Where(u => u.DisabledReason != null)
            // User has not been active in the past X months
            .Where(u => (u.LastActive != null && u.LastActive < today.AddMonths(-1 * Core.User.Consts.DeleteAfterXMonths))
                || (u.LastActive == null && u.CreatedDate < today.AddMonths(-1 * Core.User.Consts.DeleteAfterXMonths))
            )
            .ToListAsync();

        _coreContext.Users.RemoveRange(toDeleteUsers);
        await _coreContext.SaveChangesAsync();
    }

    [Function(nameof(DeleteOldTokens))]
    public async Task DeleteOldTokens([TimerTrigger(/*Daily*/ "0 0 0 * * *", RunOnStartup = Core.Debug.Consts.IsDebug)] TimerInfo timerInfo)
    {
        var today = DateOnly.FromDateTime(DateTime.UtcNow);
        var userTokensToRemove = await _coreContext.UserTokens
            // Delete expired tokens after 1 day
            .Where(u => u.Expires < today.AddDays(-1))
            .ToListAsync();

        _coreContext.UserTokens.RemoveRange(userTokensToRemove);
        await _coreContext.SaveChangesAsync();
    }
}
