using MediatR;

namespace LMS.Application.Features.Books.Commands.UploadBookCover;

/// <summary>Returns the stored blob URL.</summary>
public record UploadBookCoverCommand(
    Guid BookId,
    Stream Content,
    string ContentType,
    string FileName,
    long SizeBytes) : IRequest<string>;
