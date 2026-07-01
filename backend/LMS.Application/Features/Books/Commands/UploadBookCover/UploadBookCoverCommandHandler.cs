using LMS.Application.Common.Interfaces;
using LMS.Domain.Entities;
using LMS.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LMS.Application.Features.Books.Commands.UploadBookCover;

public class UploadBookCoverCommandHandler : IRequestHandler<UploadBookCoverCommand, string>
{
    private readonly IBookRepository _books;
    private readonly ICoverImageService _coverImages;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cache;
    private readonly ILogger<UploadBookCoverCommandHandler> _logger;

    public UploadBookCoverCommandHandler(
        IBookRepository books,
        ICoverImageService coverImages,
        IUnitOfWork unitOfWork,
        ICacheService cache,
        ILogger<UploadBookCoverCommandHandler> logger)
    {
        _books = books;
        _coverImages = coverImages;
        _unitOfWork = unitOfWork;
        _cache = cache;
        _logger = logger;
    }

    public async Task<string> Handle(UploadBookCoverCommand request, CancellationToken cancellationToken)
    {
        var book = await _books.GetByIdAsync(request.BookId, cancellationToken);
        if (book is null)
        {
            throw new NotFoundException(nameof(Book), request.BookId);
        }

        // Replacing an existing cover: remove the old blob first so it isn't orphaned.
        if (!string.IsNullOrWhiteSpace(book.CoverImageUrl))
        {
            await _coverImages.DeleteAsync(book.CoverImageUrl, cancellationToken);
        }

        var url = await _coverImages.UploadAsync(
            book.Id, request.Content, request.ContentType, request.FileName, cancellationToken);

        book.CoverImageUrl = url;
        book.UpdatedAt = DateTime.UtcNow;
        _books.Update(book);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _cache.RemoveByPrefixAsync("books:", cancellationToken);

        _logger.LogInformation("Book cover uploaded: {BookId}", book.Id);

        return url;
    }
}
