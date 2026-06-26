using MediatR;

namespace LMS.Application.Features.Books.Commands.DeleteBookCover;

public record DeleteBookCoverCommand(Guid BookId) : IRequest;
