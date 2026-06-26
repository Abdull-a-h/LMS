namespace LMS.Domain.Exceptions;

/// <summary>
/// Thrown when a domain business rule is violated (e.g. no copies available,
/// borrow limit reached). Mapped to HTTP 422 Unprocessable Entity by the API middleware.
/// </summary>
public class BusinessRuleException : Exception
{
    public BusinessRuleException(string message) : base(message) { }
}
