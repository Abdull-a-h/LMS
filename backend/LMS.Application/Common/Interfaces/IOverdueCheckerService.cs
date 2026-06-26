namespace LMS.Application.Common.Interfaces;

public interface IOverdueCheckerService
{
    Task CheckAndPublishOverdueAsync(CancellationToken cancellationToken = default);
}
