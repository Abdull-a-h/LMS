using LMS.Application.Features.Borrows.DTOs;
using MediatR;

namespace LMS.Application.Features.Borrows.Commands.ReturnBook;

public class ReturnBookCommandHandler : IRequestHandler<ReturnBookCommand, BorrowRecordDto>
{
    // TODO: record exists, belongs to current member, not already returned (else BusinessRuleException).
    //       Set ReturnedAt = now; increment AvailableCopies; log return event.
    public Task<BorrowRecordDto> Handle(ReturnBookCommand request, CancellationToken cancellationToken)
        => throw new NotImplementedException();
}
