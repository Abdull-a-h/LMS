namespace LMS.Infrastructure.Configuration;

public class OverdueCheckerOptions
{
    public const string SectionName = "OverdueChecker";

    /// <summary>How often the overdue checker runs. Defaults to 24 hours.</summary>
    public int IntervalHours { get; set; } = 24;
}
