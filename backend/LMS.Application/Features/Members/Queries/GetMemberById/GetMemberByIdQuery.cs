using LMS.Application.Features.Members.DTOs;
using MediatR;

namespace LMS.Application.Features.Members.Queries.GetMemberById;

public record GetMemberByIdQuery(Guid Id) : IRequest<MemberDetailDto>;
