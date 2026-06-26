using LMS.Application.Features.Borrows.Commands.BorrowBook;
using LMS.Application.Features.Borrows.Commands.ReturnBook;
using LMS.Application.Features.Borrows.Queries.GetAllBorrows;
using LMS.Application.Features.Borrows.Queries.GetMyBorrows;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.API.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/borrows")]
public class BorrowsController : ControllerBase
{
    private readonly IMediator _mediator;

    public BorrowsController(IMediator mediator) => _mediator = mediator;

    [HttpPost]
    [Authorize(Roles = "Member")]
    public async Task<IActionResult> Borrow(BorrowBookCommand command)
        => Ok(await _mediator.Send(command));

    [HttpPatch("{id:guid}/return")]
    [Authorize(Roles = "Member")]
    public async Task<IActionResult> Return(Guid id)
        => Ok(await _mediator.Send(new ReturnBookCommand(id)));

    [HttpGet("my")]
    [Authorize(Roles = "Member")]
    public async Task<IActionResult> GetMyBorrows([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        => Ok(await _mediator.Send(new GetMyBorrowsQuery(page, pageSize)));

    [HttpGet]
    [Authorize(Roles = "Librarian")]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        => Ok(await _mediator.Send(new GetAllBorrowsQuery(page, pageSize)));
}
