using LMS.Application.Common.Interfaces;
using LMS.Domain.Entities;
using LMS.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LMS.Application.Features.Books.Commands.DeleteBook;

public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand>
{
    private readonly IBookRepository _books;
    private readonly ICoverImageService _coverImages;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cache;
    private readonly ILogger<DeleteBookCommandHandler> _logger;

    public DeleteBookCommandHandler(
        IBookRepository books,
        ICoverImageService coverImages,
        IUnitOfWork unitOfWork,
        ICacheService cache,
        ILogger<DeleteBookCommandHandler> logger)
    {
        _books = books;
        _coverImages = coverImages;
        _unitOfWork = unitOfWork;
        _cache = cache;
        _logger = logger;
    }

    public async Task Handle(DeleteBookCommand request, CancellationToken cancellationToken)
    {
        var book = await _books.GetByIdAsync(request.Id, cancellationToken);
        if (book is null)
        {
            throw new NotFoundException(nameof(Book), request.Id);
        }

        // Available < Total means copies are out on loan — block deletion (422).
        // AvailableCopies is the maintained counter, so no need to scan borrow records.
        if (book.AvailableCopies < book.TotalCopies)
        {
            throw new BusinessRuleException("Cannot delete a book that has active borrows.");
        }

        // Remove the cover blob so storage doesn't leak orphaned files.
        if (!string.IsNullOrWhiteSpace(book.CoverImageUrl))
        {
            await _coverImages.DeleteAsync(book.CoverImageUrl, cancellationToken);
        }

        await _books.SoftDeleteAsync(request.Id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _cache.RemoveByPrefixAsync("books:", cancellationToken);

        _logger.LogInformation("Book soft-deleted: {BookId}", request.Id);
    }
}
