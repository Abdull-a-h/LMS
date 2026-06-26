using LMS.Application.Features.Borrows.DTOs;
using MediatR;

namespace LMS.Application.Features.Borrows.Commands.ReturnBook;

public record ReturnBookCommand(Guid BorrowRecordId) : IRequest<BorrowRecordDto>;
