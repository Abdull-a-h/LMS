using LMS.Application.Features.Authors.DTOs;
using MediatR;

namespace LMS.Application.Features.Authors.Commands.CreateAuthor;

public class CreateAuthorCommandHandler : IRequestHandler<CreateAuthorCommand, AuthorDto>
{
    public Task<AuthorDto> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
        => throw new NotImplementedException();
}
