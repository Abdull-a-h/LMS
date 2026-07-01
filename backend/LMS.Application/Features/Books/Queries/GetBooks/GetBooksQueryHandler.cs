using AutoMapper;
using LMS.Application.Common.Interfaces;
using LMS.Application.Common.Models;
using LMS.Application.Features.Books.DTOs;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LMS.Application.Features.Books.Queries.GetBooks;

public class GetBooksQueryHandler : IRequestHandler<GetBooksQuery, PagedResult<BookDto>>
{
    private static readonly TimeSpan CacheTtl = TimeSpan.FromMinutes(5);

    private readonly IBookRepository _books;
    private readonly ICacheService _cache;
    private readonly IMapper _mapper;
    private readonly ILogger<GetBooksQueryHandler> _logger;

    public GetBooksQueryHandler(
        IBookRepository books,
        ICacheService cache,
        IMapper mapper,
        ILogger<GetBooksQueryHandler> logger)
    {
        _books = books;
        _cache = cache;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PagedResult<BookDto>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"books:list:{request.AuthorId}:{request.Keyword}:{request.Page}:{request.PageSize}";

        var cached = await _cache.GetAsync<PagedResult<BookDto>>(cacheKey, cancellationToken);
        if (cached is not null)
        {
            _logger.LogDebug("Cache {Result} for key {CacheKey}", "HIT", cacheKey);
            return cached;
        }

        _logger.LogDebug("Cache {Result} for key {CacheKey}", "MISS", cacheKey);

        var paged = await _books.GetPagedAsync(
            request.AuthorId, request.Keyword, request.Page, request.PageSize, cancellationToken);

        var result = new PagedResult<BookDto>
        {
            Items = _mapper.Map<IReadOnlyList<BookDto>>(paged.Items),
            TotalCount = paged.TotalCount,
            Page = paged.Page,
            PageSize = paged.PageSize
        };

        await _cache.SetAsync(cacheKey, result, CacheTtl, cancellationToken);

        return result;
    }
}
