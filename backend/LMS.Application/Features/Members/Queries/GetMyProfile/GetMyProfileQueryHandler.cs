using LMS.Application.Features.Members.DTOs;
using MediatR;

namespace LMS.Application.Features.Members.Queries.GetMyProfile;

public class GetMyProfileQueryHandler : IRequestHandler<GetMyProfileQuery, MemberDetailDto>
{
    // TODO: resolve current member via ICurrentUserService; return own profile.
    public Task<MemberDetailDto> Handle(GetMyProfileQuery request, CancellationToken cancellationToken)
        => throw new NotImplementedException();
}
