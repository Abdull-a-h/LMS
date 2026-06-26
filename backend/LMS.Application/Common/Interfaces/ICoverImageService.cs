namespace LMS.Application.Common.Interfaces;

public interface ICoverImageService
{
    /// <summary>Uploads to covers/{bookId}/{guid}_{filename} and returns the blob URL.</summary>
    Task<string> UploadAsync(Guid bookId, Stream content, string contentType, string fileName, CancellationToken cancellationToken = default);
    Task DeleteAsync(string blobUrl, CancellationToken cancellationToken = default);
}
