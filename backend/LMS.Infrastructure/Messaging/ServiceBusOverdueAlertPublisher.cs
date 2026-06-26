using LMS.Application.Common.Interfaces;

namespace LMS.Infrastructure.Messaging;

/// <summary>
/// Publishes overdue-alert messages to the Azure Service Bus queue "overdue-alerts".
/// </summary>
public class ServiceBusOverdueAlertPublisher : IOverdueAlertPublisher
{
    // TODO: inject ServiceBusClient; create sender for "overdue-alerts"; serialise OverdueAlertMessage; log publish.

    public Task PublishAsync(Guid bookId, string bookTitle, string memberEmail, DateTime dueDate, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();
}
