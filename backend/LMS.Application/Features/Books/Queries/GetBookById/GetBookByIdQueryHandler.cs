using AutoMapper;
using LMS.Application.Common.Interfaces;
using LMS.Application.Features.Books.DTOs;
using LMS.Domain.Entities;
using LMS.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LMS.Application.Features.Books.Queries.GetBookById;

public class GetBookByIdQueryHandler : IRequestHandler<GetBookByIdQuery, BookDetailDto>
{
    private static readonly TimeSpan CacheTtl = TimeSpan.FromMinutes(10);

    private readonly IBookRepository _books;
    private readonly ICacheService _cache;
    private readonly IMapper _mapper;
    private readonly ILogger<GetBookByIdQueryHandler> _logger;

    public GetBookByIdQueryHandler(
        IBookRepository books,
        ICacheService cache,
        IMapper mapper,
        ILogger<GetBookByIdQueryHandler> logger)
    {
        _books = books;
        _cache = cache;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<BookDetailDto> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"books:detail:{request.Id}";

        // The cached object holds only the book's shared data. The per-member
        // HasActiveBorrowByCurrentMember flag is computed after retrieval (Step 5),
        // never stored in the shared cache entry.
        var cached = await _cache.GetAsync<BookDetailDto>(cacheKey, cancellationToken);
        if (cached is not null)
        {
            _logger.LogDebug("Cache {Result} for key {CacheKey}", "HIT", cacheKey);
            return cached;
        }

        _logger.LogDebug("Cache {Result} for key {CacheKey}", "MISS", cacheKey);

        var book = await _books.GetByIdAsync(request.Id, cancellationToken);
        if (book is null)
        {
            throw new NotFoundException(nameof(Book), request.Id);
        }

        var dto = _mapper.Map<BookDetailDto>(book);

        await _cache.SetAsync(cacheKey, dto, CacheTtl, cancellationToken);

        return dto;
    }
}
