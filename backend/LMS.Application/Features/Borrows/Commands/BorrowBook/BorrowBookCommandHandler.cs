using LMS.Application.Features.Borrows.DTOs;
using MediatR;

namespace LMS.Application.Features.Borrows.Commands.BorrowBook;

public class BorrowBookCommandHandler : IRequestHandler<BorrowBookCommand, BorrowRecordDto>
{
    // TODO: checks — book exists & active; a copy available; member has < 3 active borrows (else BusinessRuleException).
    //       Create record DueDate = now + 14 days; decrement AvailableCopies; log borrow event.
    public Task<BorrowRecordDto> Handle(BorrowBookCommand request, CancellationToken cancellationToken)
        => throw new NotImplementedException();
}
