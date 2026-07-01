using LMS.Application.Common.Interfaces;
using LMS.Domain.Entities;
using LMS.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LMS.Application.Features.Books.Commands.DeleteBookCover;

public class DeleteBookCoverCommandHandler : IRequestHandler<DeleteBookCoverCommand>
{
    private readonly IBookRepository _books;
    private readonly ICoverImageService _coverImages;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cache;
    private readonly ILogger<DeleteBookCoverCommandHandler> _logger;

    public DeleteBookCoverCommandHandler(
        IBookRepository books,
        ICoverImageService coverImages,
        IUnitOfWork unitOfWork,
        ICacheService cache,
        ILogger<DeleteBookCoverCommandHandler> logger)
    {
        _books = books;
        _coverImages = coverImages;
        _unitOfWork = unitOfWork;
        _cache = cache;
        _logger = logger;
    }

    public async Task Handle(DeleteBookCoverCommand request, CancellationToken cancellationToken)
    {
        var book = await _books.GetByIdAsync(request.BookId, cancellationToken);
        if (book is null)
        {
            throw new NotFoundException(nameof(Book), request.BookId);
        }

        // Nothing to do if there is no cover.
        if (string.IsNullOrWhiteSpace(book.CoverImageUrl))
        {
            return;
        }

        await _coverImages.DeleteAsync(book.CoverImageUrl, cancellationToken);

        book.CoverImageUrl = null;
        book.UpdatedAt = DateTime.UtcNow;
        _books.Update(book);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _cache.RemoveByPrefixAsync("books:", cancellationToken);

        _logger.LogInformation("Book cover removed: {BookId}", book.Id);
    }
}
