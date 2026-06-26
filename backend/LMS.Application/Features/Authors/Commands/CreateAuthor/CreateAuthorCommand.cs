using LMS.Application.Features.Authors.DTOs;
using MediatR;

namespace LMS.Application.Features.Authors.Commands.CreateAuthor;

public record CreateAuthorCommand(string Name, string? Biography, string? Nationality) : IRequest<AuthorDto>;
