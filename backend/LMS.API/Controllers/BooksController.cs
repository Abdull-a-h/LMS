using LMS.Application.Features.Books.Commands.CreateBook;
using LMS.Application.Features.Books.Commands.DeleteBook;
using LMS.Application.Features.Books.Commands.DeleteBookCover;
using LMS.Application.Features.Books.Commands.UpdateBook;
using LMS.Application.Features.Books.Commands.UploadBookCover;
using LMS.Application.Features.Books.Queries.GetBookById;
using LMS.Application.Features.Books.Queries.GetBooks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LMS.API.Controllers;

[ApiController]
[Authorize]
[Route("api/v1/books")]
public class BooksController : ControllerBase
{
    private readonly IMediator _mediator;

    public BooksController(IMediator mediator) => _mediator = mediator;

    [HttpGet]
    public async Task<IActionResult> GetBooks([FromQuery] Guid? authorId, [FromQuery] string? q, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        => Ok(await _mediator.Send(new GetBooksQuery(authorId, q, page, pageSize)));

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
        => Ok(await _mediator.Send(new GetBookByIdQuery(id)));

    [HttpPost]
    [Authorize(Roles = "Librarian")]
    public async Task<IActionResult> Create(CreateBookCommand command)
        => Ok(await _mediator.Send(command));

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Librarian")]
    public async Task<IActionResult> Update(Guid id, UpdateBookCommand command)
        => Ok(await _mediator.Send(command with { Id = id }));

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Librarian")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _mediator.Send(new DeleteBookCommand(id));
        return NoContent();
    }

    [HttpPost("{id:guid}/cover")]
    [Authorize(Roles = "Librarian")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadCover(Guid id, IFormFile file)
    {
        await using var stream = file.OpenReadStream();
        var url = await _mediator.Send(new UploadBookCoverCommand(id, stream, file.ContentType, file.FileName, file.Length));
        return Ok(new { coverImageUrl = url });
    }

    [HttpDelete("{id:guid}/cover")]
    [Authorize(Roles = "Librarian")]
    public async Task<IActionResult> DeleteCover(Guid id)
    {
        await _mediator.Send(new DeleteBookCoverCommand(id));
        return NoContent();
    }
}
