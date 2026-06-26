using LMS.Application.Features.Authors.DTOs;
using MediatR;

namespace LMS.Application.Features.Authors.Commands.UpdateAuthor;

public record UpdateAuthorCommand(Guid Id, string Name, string? Biography, string? Nationality) : IRequest<AuthorDto>;
