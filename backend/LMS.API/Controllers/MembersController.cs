using LMS.Application.Features.Members.Commands.DeactivateMember;
using LMS.Application.Features.Members.Queries.GetMemberById;
using LMS.Application.Features.Members.Queries.GetMembers;
using LMS.Application.Features.Members.Queries.GetMyProfile;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.API.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/members")]
public class MembersController : ControllerBase
{
    private readonly IMediator _mediator;

    public MembersController(IMediator mediator) => _mediator = mediator;

    [HttpGet("me")]
    [Authorize(Roles = "Member")]
    public async Task<IActionResult> GetMyProfile()
        => Ok(await _mediator.Send(new GetMyProfileQuery()));

    [HttpGet]
    [Authorize(Roles = "Librarian")]
    public async Task<IActionResult> GetMembers([FromQuery] string? q, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        => Ok(await _mediator.Send(new GetMembersQuery(q, page, pageSize)));

    [HttpGet("{id:guid}")]
    [Authorize(Roles = "Librarian")]
    public async Task<IActionResult> GetById(Guid id)
        => Ok(await _mediator.Send(new GetMemberByIdQuery(id)));

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Librarian")]
    public async Task<IActionResult> Deactivate(Guid id)
    {
        await _mediator.Send(new DeactivateMemberCommand(id));
        return NoContent();
    }
}
