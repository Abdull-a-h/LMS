using LMS.Application.Features.Borrows.DTOs;
using MediatR;

namespace LMS.Application.Features.Borrows.Commands.BorrowBook;

public record BorrowBookCommand(Guid BookId) : IRequest<BorrowRecordDto>;
