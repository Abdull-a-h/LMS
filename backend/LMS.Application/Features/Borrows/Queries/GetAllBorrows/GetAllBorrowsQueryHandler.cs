using LMS.Application.Common.Models;
using LMS.Application.Features.Borrows.DTOs;
using MediatR;

namespace LMS.Application.Features.Borrows.Queries.GetAllBorrows;

public class GetAllBorrowsQueryHandler : IRequestHandler<GetAllBorrowsQuery, PagedResult<BorrowRecordDto>>
{
    // TODO: all borrow records (Librarian view), paginated, mapped via AutoMapper.
    public Task<PagedResult<BorrowRecordDto>> Handle(GetAllBorrowsQuery request, CancellationToken cancellationToken)
        => throw new NotImplementedException();
}
