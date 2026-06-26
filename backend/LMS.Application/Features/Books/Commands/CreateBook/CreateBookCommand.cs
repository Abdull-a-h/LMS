using LMS.Application.Features.Books.DTOs;
using MediatR;

namespace LMS.Application.Features.Books.Commands.CreateBook;

public record CreateBookCommand(
    string Title,
    string? Description,
    string ISBN,
    int PublicationYear,
    int TotalCopies,
    Guid AuthorId) : IRequest<BookDto>;
