using LMS.Application.Common.Models;
using LMS.Application.Features.Members.DTOs;
using MediatR;

namespace LMS.Application.Features.Members.Queries.GetMembers;

public record GetMembersQuery(string? Keyword, int Page = 1, int PageSize = 10) : IRequest<PagedResult<MemberSummaryDto>>;
