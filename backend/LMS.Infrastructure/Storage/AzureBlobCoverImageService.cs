using LMS.Application.Common.Interfaces;

namespace LMS.Infrastructure.Storage;

/// <summary>
/// Uploads/deletes book cover images in Azure Blob Storage (Azurite locally).
/// Blob path: covers/{bookId}/{guid}_{filename}.
/// </summary>
public class AzureBlobCoverImageService : ICoverImageService
{
    // TODO: inject BlobServiceClient + container name from options; ensure container exists.

    public Task<string> UploadAsync(Guid bookId, Stream content, string contentType, string fileName, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    public Task DeleteAsync(string blobUrl, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();
}
