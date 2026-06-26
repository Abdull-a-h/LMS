using FluentValidation;

namespace LMS.Application.Features.Books.Commands.UploadBookCover;

public class UploadBookCoverCommandValidator : AbstractValidator<UploadBookCoverCommand>
{
    public UploadBookCoverCommandValidator()
    {
        // TODO: BookId required. ContentType must be an image. SizeBytes max 8 MB.
    }
}
