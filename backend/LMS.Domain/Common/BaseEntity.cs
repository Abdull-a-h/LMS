namespace LMS.Domain.Common;

/// <summary>
/// Base type for all persisted entities. Pure C# — no infrastructure concerns.
/// </summary>
public abstract class BaseEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
}
