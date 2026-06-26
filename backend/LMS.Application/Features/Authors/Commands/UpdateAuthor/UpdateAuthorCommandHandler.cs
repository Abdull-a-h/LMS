using LMS.Application.Features.Authors.DTOs;
using MediatR;

namespace LMS.Application.Features.Authors.Commands.UpdateAuthor;

public class UpdateAuthorCommandHandler : IRequestHandler<UpdateAuthorCommand, AuthorDto>
{
    public Task<AuthorDto> Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
        => throw new NotImplementedException();
}
