using LMS.Application.Common.Models;
using LMS.Application.Features.Borrows.DTOs;
using MediatR;

namespace LMS.Application.Features.Borrows.Queries.GetAllBorrows;

public record GetAllBorrowsQuery(int Page = 1, int PageSize = 10) : IRequest<PagedResult<BorrowRecordDto>>;
