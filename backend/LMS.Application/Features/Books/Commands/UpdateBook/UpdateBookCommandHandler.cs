using AutoMapper;
using LMS.Application.Common.Interfaces;
using LMS.Application.Features.Books.DTOs;
using LMS.Domain.Entities;
using LMS.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LMS.Application.Features.Books.Commands.UpdateBook;

public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, BookDto>
{
    private readonly IBookRepository _books;
    private readonly IAuthorRepository _authors;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cache;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateBookCommandHandler> _logger;

    public UpdateBookCommandHandler(
        IBookRepository books,
        IAuthorRepository authors,
        IUnitOfWork unitOfWork,
        ICacheService cache,
        IMapper mapper,
        ILogger<UpdateBookCommandHandler> logger)
    {
        _books = books;
        _authors = authors;
        _unitOfWork = unitOfWork;
        _cache = cache;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<BookDto> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
    {
        var book = await _books.GetByIdAsync(request.Id, cancellationToken);
        if (book is null)
        {
            throw new NotFoundException(nameof(Book), request.Id);
        }

        var author = await _authors.GetByIdAsync(request.AuthorId, cancellationToken);
        if (author is null)
        {
            throw new NotFoundException(nameof(Author), request.AuthorId);
        }

        // ISBN must stay unique across all books except this one.
        if (await _books.IsbnExistsAsync(request.ISBN, book.Id, cancellationToken))
        {
            throw new BusinessRuleException($"A book with ISBN {request.ISBN} already exists.");
        }

        // Copies currently out on loan = Total − Available. The new total cannot drop below it.
        var borrowedCount = book.TotalCopies - book.AvailableCopies;
        if (request.TotalCopies < borrowedCount)
        {
            throw new BusinessRuleException(
                $"Total copies cannot be less than the {borrowedCount} copy(ies) currently borrowed.");
        }

        book.Title = request.Title.Trim();
        book.Description = request.Description?.Trim();
        book.ISBN = request.ISBN;
        book.PublicationYear = request.PublicationYear;
        book.TotalCopies = request.TotalCopies;
        book.AvailableCopies = request.TotalCopies - borrowedCount; // preserve outstanding loans
        book.AuthorId = author.Id;
        book.Author = author;
        book.UpdatedAt = DateTime.UtcNow;

        _books.Update(book);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _cache.RemoveByPrefixAsync("books:", cancellationToken);

        _logger.LogInformation("Book updated: {BookId}", book.Id);

        return _mapper.Map<BookDto>(book);
    }
}
