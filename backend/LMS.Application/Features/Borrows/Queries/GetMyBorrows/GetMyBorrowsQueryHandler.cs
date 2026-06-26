using LMS.Application.Common.Models;
using LMS.Application.Features.Borrows.DTOs;
using MediatR;

namespace LMS.Application.Features.Borrows.Queries.GetMyBorrows;

public class GetMyBorrowsQueryHandler : IRequestHandler<GetMyBorrowsQuery, PagedResult<BorrowRecordDto>>
{
    // TODO: current member's borrow records (active + historical), paginated, mapped via AutoMapper.
    public Task<PagedResult<BorrowRecordDto>> Handle(GetMyBorrowsQuery request, CancellationToken cancellationToken)
        => throw new NotImplementedException();
}
