using AutoMapper;
using LMS.Application.Common.Interfaces;
using LMS.Application.Features.Authors.DTOs;
using LMS.Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LMS.Application.Features.Authors.Commands.CreateAuthor;

public class CreateAuthorCommandHandler : IRequestHandler<CreateAuthorCommand, AuthorDto>
{
    private readonly IAuthorRepository _authors;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateAuthorCommandHandler> _logger;

    public CreateAuthorCommandHandler(
        IAuthorRepository authors,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<CreateAuthorCommandHandler> logger)
    {
        _authors = authors;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<AuthorDto> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
    {
        var author = new Author
        {
            Id = Guid.NewGuid(),
            Name = request.Name.Trim(),
            Biography = request.Biography?.Trim(),
            Nationality = request.Nationality?.Trim(),
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        await _authors.CreateAsync(author, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Author created: {AuthorId}", author.Id);

        return _mapper.Map<AuthorDto>(author);
    }
}
