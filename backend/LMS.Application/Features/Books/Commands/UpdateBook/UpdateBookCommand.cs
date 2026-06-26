using LMS.Application.Features.Books.DTOs;
using MediatR;

namespace LMS.Application.Features.Books.Commands.UpdateBook;

public record UpdateBookCommand(
    Guid Id,
    string Title,
    string? Description,
    string ISBN,
    int PublicationYear,
    int TotalCopies,
    Guid AuthorId) : IRequest<BookDto>;
