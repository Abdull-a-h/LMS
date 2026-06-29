using LMS.Application.Common.Interfaces;
using LMS.Domain.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LMS.Application.Features.Authors.Commands.DeleteAuthor;

public class DeleteAuthorCommandHandler : IRequestHandler<DeleteAuthorCommand>
{
    private readonly IAuthorRepository _authors;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteAuthorCommandHandler> _logger;

    public DeleteAuthorCommandHandler(
        IAuthorRepository authors,
        IUnitOfWork unitOfWork,
        ILogger<DeleteAuthorCommandHandler> logger)
    {
        _authors = authors;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Handle(DeleteAuthorCommand request, CancellationToken cancellationToken)
    {
        // GetByIdAsync includes the author's active books (Book IsActive query filter).
        var author = await _authors.GetByIdAsync(request.Id, cancellationToken);
        if (author is null)
        {
            throw new NotFoundException(nameof(LMS.Domain.Entities.Author), request.Id);
        }

        // Business rule: an author still linked to active books cannot be removed.
        // (FK is DeleteBehavior.Restrict; this surfaces a clean 422 instead of a DB error.)
        if (author.Books.Any())
        {
            throw new BusinessRuleException("Cannot delete an author who still has active books.");
        }

        await _authors.SoftDeleteAsync(request.Id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Author soft-deleted: {AuthorId}", request.Id);
    }
}
