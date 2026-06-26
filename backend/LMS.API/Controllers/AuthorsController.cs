using LMS.Application.Features.Authors.Commands.CreateAuthor;
using LMS.Application.Features.Authors.Commands.DeleteAuthor;
using LMS.Application.Features.Authors.Commands.UpdateAuthor;
using LMS.Application.Features.Authors.Queries.GetAuthorById;
using LMS.Application.Features.Authors.Queries.GetAuthors;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.API.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/authors")]
public class AuthorsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthorsController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetAll()
        => Ok(await _mediator.Send(new GetAuthorsQuery()));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
        => Ok(await _mediator.Send(new GetAuthorByIdQuery(id)));

    [HttpPost]
    [Authorize(Roles = "Librarian")]
    public async Task<IActionResult> Create(CreateAuthorCommand command)
        => Ok(await _mediator.Send(command));

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Librarian")]
    public async Task<IActionResult> Update(Guid id, UpdateAuthorCommand command)
        => Ok(await _mediator.Send(command with { Id = id }));

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Librarian")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteAuthorCommand(id));
        return NoContent();
    }
}
