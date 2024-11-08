using System.Text;
using System.Text.Json;
using Epicty.EmailNotifier.EmailNotification;
using Epicty.EmailNotifier.Models;
using Epicty.EmailNotifier.Models.Requests;
using Microsoft.Extensions.Options;

namespace Epicty.EmailNotifier.Endpoints;

internal static class EpicfyEndpoints
{
    internal static IEndpointRouteBuilder AddEpicfyEndpoints(this IEndpointRouteBuilder endpoints)
    {
        RouteGroupBuilder epicfyEndpoints =
            endpoints.MapGroup("api/v1/email-notifications/").WithTags("Send Email Endpoints!");

        epicfyEndpoints.MapPost("new-idea", HandleSendEmailIdea);
        epicfyEndpoints.MapPost("confirm-email", HandleSendConfirmationEmail);

        return epicfyEndpoints;
    }

    #region [Private Methods]

    private static async Task<T> ReadBodyAs<T>(HttpContext context)
    {
        context.Request.EnableBuffering();
        using StreamReader reader = new(context.Request.Body, Encoding.UTF8, leaveOpen: true);

        string bodyString = await reader.ReadToEndAsync();
        context.Request.Body.Position = 0;
        return JsonSerializer.Deserialize<T>(bodyString)!;
    }

    #endregion
    #region [Handlers]

    private static IResult HandleSendConfirmationEmail(EmailConfirmationRequest emailConfirmationRequest,
        ILogger<Program> logger, IOptions<SmtpSettings> smtpSettings)
    {
        try
        {
            EmailConfirmation emailConfirmation = new EmailConfirmation()
                .WithTargetEmail(emailConfirmationRequest.TargetEmail)
                .WithConfirmationUrl(emailConfirmationRequest.ConfirmationUrl)
                .WithUserName(emailConfirmationRequest.UserName);
            
            emailConfirmation.Send(smtpSettings.Value);
            logger.LogInformation($"Notificação enviada com sucesso para: {emailConfirmationRequest.TargetEmail}");
            
            return Results.Ok();
        }
        catch (Exception error)
        {
            logger.LogError($"Erro ao enviar notificação: {error}");
            return Results.BadRequest($"Erro ao enviar notificação: {error}");
        }
    }

    private static IResult HandleSendEmailIdea(NewIdeaRequest newIdeaRequest, ILogger<Program> logger,
        IOptions<SmtpSettings> smtpSettings)
    {
        try
        {
            NewIdeaEmailNotification emailConfirmation = new NewIdeaEmailNotification()
                .WithTargetEmail(newIdeaRequest.TargetEmail)
                .WithUserName(newIdeaRequest.CreatedBy)
                .WithIdeaTitle(newIdeaRequest.Idea.Title)
                .WithIdeaDescription(newIdeaRequest.Idea.Description)
                .WithCreatedAt(newIdeaRequest.Idea.CreatedAt);

            emailConfirmation.Send(smtpSettings.Value);
            logger.LogInformation($"Notificação enviada com sucesso para: {newIdeaRequest.TargetEmail}");

            return Results.Ok();
        }
        catch (Exception error)
        {
            logger.LogError($"Erro ao enviar notificação: {error}");
            return Results.BadRequest($"Erro ao enviar notificação: {error}");
        }
    }

    #endregion
}