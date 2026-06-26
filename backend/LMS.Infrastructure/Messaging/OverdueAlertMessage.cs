namespace LMS.Infrastructure.Messaging;

/// <summary>Payload published to / consumed from the "overdue-alerts" Service Bus queue.</summary>
public record OverdueAlertMessage
{
    public Guid BorrowRecordId { get; init; }
    public Guid BookId { get; init; }
    public string BookTitle { get; init; } = string.Empty;
    public string MemberEmail { get; init; } = string.Empty;
    public string MemberName { get; init; } = string.Empty;
    public DateTime DueDate { get; init; }
    public int DaysOverdue { get; init; }
}
