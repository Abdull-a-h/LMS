using MediatR;

namespace LMS.Application.Features.Authors.Commands.DeleteAuthor;

public record DeleteAuthorCommand(Guid Id) : IRequest;
