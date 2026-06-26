using MediatR;

namespace LMS.Application.Features.Authentication.Commands.Logout;

public record LogoutCommand(string RefreshToken) : IRequest;
