using FluentValidation;

namespace LMS.Application.Features.Books.Commands.UploadBookCover;

public class UploadBookCoverCommandValidator : AbstractValidator<UploadBookCoverCommand>
{
    // 8 MB upload ceiling.
    private const long MaxSizeBytes = 8 * 1024 * 1024;

    private static readonly string[] AllowedContentTypes =
        { "image/jpeg", "image/png", "image/webp", "image/gif" };

    public UploadBookCoverCommandValidator()
    {
        RuleFor(x => x.BookId)
            .NotEmpty().WithMessage("Book id is required.");

        RuleFor(x => x.ContentType)
            .Must(ct => ct is not null && AllowedContentTypes.Contains(ct.ToLowerInvariant()))
            .WithMessage("Cover must be a JPEG, PNG, WebP, or GIF image.");

        RuleFor(x => x.SizeBytes)
            .GreaterThan(0).WithMessage("Uploaded file is empty.")
            .LessThanOrEqualTo(MaxSizeBytes).WithMessage("Cover image must not exceed 8 MB.");
    }
}
