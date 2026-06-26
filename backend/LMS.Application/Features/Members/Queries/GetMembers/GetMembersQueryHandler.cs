using LMS.Application.Common.Models;
using LMS.Application.Features.Members.DTOs;
using MediatR;

namespace LMS.Application.Features.Members.Queries.GetMembers;

public class GetMembersQueryHandler : IRequestHandler<GetMembersQuery, PagedResult<MemberSummaryDto>>
{
    // TODO: paginated, searchable by name/email, mapped via AutoMapper.
    public Task<PagedResult<MemberSummaryDto>> Handle(GetMembersQuery request, CancellationToken cancellationToken)
        => throw new NotImplementedException();
}
