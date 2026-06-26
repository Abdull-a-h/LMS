using Microsoft.Extensions.Hosting;

namespace LMS.Infrastructure.Messaging;

/// <summary>
/// Subscribes to the "overdue-alerts" queue at startup. Logs each message at Warning level
/// via Serilog; dead-letters messages that cannot be deserialised (logged at Error).
/// </summary>
public class OverdueAlertConsumerService : BackgroundService
{
    // TODO: inject ServiceBusClient + ILogger; create ServiceBusProcessor; wire message/error handlers.

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
        => throw new NotImplementedException();
}
