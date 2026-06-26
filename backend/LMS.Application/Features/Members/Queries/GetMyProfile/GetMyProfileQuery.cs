using LMS.Application.Features.Members.DTOs;
using MediatR;

namespace LMS.Application.Features.Members.Queries.GetMyProfile;

public record GetMyProfileQuery : IRequest<MemberDetailDto>;
