using AutoMapper;
using LMS.Application.Common.Interfaces;
using LMS.Application.Features.Books.DTOs;
using LMS.Domain.Entities;
using LMS.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LMS.Application.Features.Books.Commands.CreateBook;

public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, BookDto>
{
    private readonly IBookRepository _books;
    private readonly IAuthorRepository _authors;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheService _cache;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateBookCommandHandler> _logger;

    public CreateBookCommandHandler(
        IBookRepository books,
        IAuthorRepository authors,
        IUnitOfWork unitOfWork,
        ICacheService cache,
        IMapper mapper,
        ILogger<CreateBookCommandHandler> logger)
    {
        _books = books;
        _authors = authors;
        _unitOfWork = unitOfWork;
        _cache = cache;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<BookDto> Handle(CreateBookCommand request, CancellationToken cancellationToken)
    {
        // The referenced author must exist (FK is Restrict; this gives a clean 404 instead of a DB error).
        var author = await _authors.GetByIdAsync(request.AuthorId, cancellationToken);
        if (author is null)
        {
            throw new NotFoundException(nameof(Author), request.AuthorId);
        }

        if (await _books.IsbnExistsAsync(request.ISBN, null, cancellationToken))
        {
            throw new BusinessRuleException($"A book with ISBN {request.ISBN} already exists.");
        }

        var book = new Book
        {
            Id = Guid.NewGuid(),
            Title = request.Title.Trim(),
            Description = request.Description?.Trim(),
            ISBN = request.ISBN,
            PublicationYear = request.PublicationYear,
            TotalCopies = request.TotalCopies,
            AvailableCopies = request.TotalCopies, // all copies available at creation
            AuthorId = author.Id,
            Author = author, // so the response DTO can flatten AuthorName
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await _books.CreateAsync(book, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // A new book changes list results — drop every cached "books:" entry.
        await _cache.RemoveByPrefixAsync("books:", cancellationToken);

        _logger.LogInformation("Book created: {BookId}", book.Id);

        return _mapper.Map<BookDto>(book);
    }
}
