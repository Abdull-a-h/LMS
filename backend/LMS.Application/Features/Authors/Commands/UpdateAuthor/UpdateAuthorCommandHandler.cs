using AutoMapper;
using LMS.Application.Common.Interfaces;
using LMS.Application.Features.Authors.DTOs;
using LMS.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LMS.Application.Features.Authors.Commands.UpdateAuthor;

public class UpdateAuthorCommandHandler : IRequestHandler<UpdateAuthorCommand, AuthorDto>
{
    private readonly IAuthorRepository _authors;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILogger<UpdateAuthorCommandHandler> _logger;

    public UpdateAuthorCommandHandler(
        IAuthorRepository authors,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<UpdateAuthorCommandHandler> logger)
    {
        _authors = authors;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<AuthorDto> Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
    {
        var author = await _authors.GetByIdAsync(request.Id, cancellationToken);
        if (author is null)
        {
            throw new NotFoundException(nameof(LMS.Domain.Entities.Author), request.Id);
        }

        author.Name = request.Name.Trim();
        author.Biography = request.Biography?.Trim();
        author.Nationality = request.Nationality?.Trim();

        _authors.Update(author);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Author updated: {AuthorId}", author.Id);

        return _mapper.Map<AuthorDto>(author);
    }
}
