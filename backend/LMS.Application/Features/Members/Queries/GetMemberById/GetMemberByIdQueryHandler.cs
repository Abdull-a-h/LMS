using LMS.Application.Features.Members.DTOs;
using MediatR;

namespace LMS.Application.Features.Members.Queries.GetMemberById;

public class GetMemberByIdQueryHandler : IRequestHandler<GetMemberByIdQuery, MemberDetailDto>
{
    // TODO: full profile + active borrow count + last 10 borrow records (NotFound if missing).
    public Task<MemberDetailDto> Handle(GetMemberByIdQuery request, CancellationToken cancellationToken)
        => throw new NotImplementedException();
}
