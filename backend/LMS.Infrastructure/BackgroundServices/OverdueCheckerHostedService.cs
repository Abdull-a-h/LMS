using Microsoft.Extensions.Hosting;

namespace LMS.Infrastructure.BackgroundServices;

/// <summary>
/// Runs IOverdueCheckerService.CheckAndPublishOverdueAsync() on a timer (default every 24h,
/// configurable via appsettings). Resolves the scoped checker from IServiceScopeFactory per tick.
/// </summary>
public class OverdueCheckerHostedService : BackgroundService
{
    // TODO: inject IServiceScopeFactory + IOptions<OverdueCheckerOptions> + ILogger.

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
        => throw new NotImplementedException();
}
