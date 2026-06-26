namespace LMS.Infrastructure.Configuration;

public class ServiceBusOptions
{
    public const string SectionName = "ServiceBus";

    public string ConnectionString { get; set; } = string.Empty;
    public string OverdueQueueName { get; set; } = "overdue-alerts";
}
