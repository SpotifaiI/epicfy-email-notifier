namespace Epicty.EmailNotifier.Models;

public class ErrorResponse(int statusCode, List<string> errors)
{
    public int StatusCode { get; set; } = statusCode;
    public List<string> Errors { get; set; } = errors;
}