using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using LMS.Application.Common.Interfaces;
using LMS.Infrastructure.Configuration;
using Microsoft.Extensions.Options;

namespace LMS.Infrastructure.Storage;

/// <summary>
/// Uploads/deletes book cover images in Azure Blob Storage (Azurite locally).
/// Blob path: covers/{bookId}/{guid}_{filename}. Switching to real Azure is a
/// connection-string change only — no code change.
/// </summary>
public class AzureBlobCoverImageService : ICoverImageService
{
    private readonly BlobServiceClient _blobServiceClient;
    private readonly string _containerName;

    public AzureBlobCoverImageService(BlobServiceClient blobServiceClient, IOptions<BlobStorageOptions> options)
    {
        _blobServiceClient = blobServiceClient;
        _containerName = options.Value.ContainerName;
    }

    public async Task<string> UploadAsync(
        Guid bookId, Stream content, string contentType, string fileName, CancellationToken cancellationToken = default)
    {
        var container = _blobServiceClient.GetBlobContainerClient(_containerName);

        // Public blob access so the returned URL renders directly in the browser (dev/Azurite).
        await container.CreateIfNotExistsAsync(PublicAccessType.Blob, cancellationToken: cancellationToken);

        // Path within the "covers" container: {bookId}/{guid}_{filename}. The guid prevents
        // collisions when the same file name is uploaded twice.
        var safeName = Path.GetFileName(fileName);
        var blobName = $"{bookId}/{Guid.NewGuid()}_{safeName}";
        var blob = container.GetBlobClient(blobName);

        await blob.UploadAsync(
            content,
            new BlobUploadOptions { HttpHeaders = new BlobHttpHeaders { ContentType = contentType } },
            cancellationToken);

        return blob.Uri.ToString();
    }

    public async Task DeleteAsync(string blobUrl, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(blobUrl))
        {
            return;
        }

        // Recover the blob name (path within the container) from the stored URL, then delete
        // through the authenticated container client rather than the raw (credential-less) URI.
        var builder = new BlobUriBuilder(new Uri(blobUrl));
        var container = _blobServiceClient.GetBlobContainerClient(builder.BlobContainerName);
        await container.GetBlobClient(builder.BlobName).DeleteIfExistsAsync(cancellationToken: cancellationToken);
    }
}
