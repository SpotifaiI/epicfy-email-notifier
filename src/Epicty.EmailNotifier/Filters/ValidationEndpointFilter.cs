using Epicty.EmailNotifier.EmailNotification;
using Epicty.EmailNotifier.Models;
using Epicty.EmailNotifier.Models.Requests;
using Epicty.EmailNotifier.Models.Responses;

namespace Epicty.EmailNotifier.Filters;

public class ValidationEndpointFilter<T> : IEndpointFilter where T : class
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        List<string> validationErrors = [];
        try
        {
            if (context.Arguments.FirstOrDefault(arg => arg is T) is not T request)
            {
                validationErrors.Add("Request is null or invalid.");
            }
            else
            {
                if (typeof(T) == typeof(NewIdeaRequest))
                {
                    NewIdeaEmailNotification notification = new();
                    List<string> errors = notification.Validate((NewIdeaRequest)(object)request);
                    if (errors.Count > 0)
                    {
                        validationErrors.AddRange(errors);
                    }
                }
                else if (typeof(T) == typeof(EmailConfirmationRequest))
                {
                    EmailConfirmationNotification notification = new();
                    List<string> errors = notification.Validate((EmailConfirmationRequest)(object)request);
                    if (errors.Count > 0)
                    {
                        validationErrors.AddRange(errors);
                    }
                }
                else
                {
                    validationErrors.Add($"No validation logic found for request type '{typeof(T).Name}'.");
                }
            }
        }
        catch (Exception ex)
        {
            validationErrors.Add($"Error processing request: {ex.Message}");
        }

        if (validationErrors.Count <= 0)
        {
            return await next(context);
        }

        context.HttpContext.Response.StatusCode = 400;
        context.HttpContext.Response.ContentType = "application/json";
        return Results.BadRequest(new ErrorResponse(400, validationErrors));
    }
}