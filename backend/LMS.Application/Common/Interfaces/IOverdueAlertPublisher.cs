namespace LMS.Application.Common.Interfaces;

public interface IOverdueAlertPublisher
{
    Task PublishAsync(Guid bookId, string bookTitle, string memberEmail, DateTime dueDate, CancellationToken cancellationToken = default);
}
