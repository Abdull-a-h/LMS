using LMS.Application.Features.Authentication.DTOs;
using MediatR;

namespace LMS.Application.Features.Authentication.Commands.Register;

public record RegisterCommand(string FullName, string Email, string Password) : IRequest<AuthResultDto>;
