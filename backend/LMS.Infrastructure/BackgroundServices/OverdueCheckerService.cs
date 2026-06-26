using LMS.Application.Common.Interfaces;

namespace LMS.Infrastructure.BackgroundServices;

/// <summary>
/// Queries overdue borrow records and publishes an alert per record via IOverdueAlertPublisher.
/// </summary>
public class OverdueCheckerService : IOverdueCheckerService
{
    // TODO: inject IBorrowRepository + IOverdueAlertPublisher + ILogger.

    public Task CheckAndPublishOverdueAsync(CancellationToken cancellationToken = default)
        => throw new NotImplementedException();
}
