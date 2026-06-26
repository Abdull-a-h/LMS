namespace LMS.API.Models;

public class ErrorResponse
{
    public bool Success { get; set; }
    public int StatusCode { get; set; }
    public string Error { get; set; } = string.Empty;
    public string? TraceId { get; set; }
    public IEnumerable<ValidationError>? Errors { get; set; }
}

public record ValidationError(string Field, string Message);
