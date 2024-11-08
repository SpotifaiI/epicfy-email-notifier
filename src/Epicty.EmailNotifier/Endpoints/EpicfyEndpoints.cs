using Epicty.EmailNotifier.EmailNotification;
using Epicty.EmailNotifier.Filters;
using Epicty.EmailNotifier.Models;
using Epicty.EmailNotifier.Models.Requests;
using Epicty.EmailNotifier.Models.Responses;
using Microsoft.Extensions.Options;

namespace Epicty.EmailNotifier.Endpoints;

internal static class EpicfyEndpoints
{
    internal static IEndpointRouteBuilder AddEpicfyEndpoints(this IEndpointRouteBuilder endpoints)
    {
        RouteGroupBuilder epicfyEndpoints =endpoints.MapGroup("api/v1/email-notifications/")
            .WithTags("Send Email Endpoints!");

        epicfyEndpoints.MapPost("new-idea", HandleSendEmailIdea)
            .AddEndpointFilter<ValidationEndpointFilter<NewIdeaRequest>>()
            .Produces(500, typeof(ErrorResponse))
            .Produces(200)
            .WithOpenApi(x =>
            {
                x.Summary = "Endpoint to send an email notification for a new idea!";
                return x;
            });
        
        epicfyEndpoints.MapPost("confirm-email", HandleSendConfirmationEmail)
            .AddEndpointFilter<ValidationEndpointFilter<EmailConfirmationRequest>>()
            .Produces(500, typeof(ErrorResponse))
            .Produces(200)
            .WithOpenApi(x =>
            {
                x.Summary = "Endpoint to send an email confirmation to user!";
                return x;
            });

        return epicfyEndpoints;
    }

    #region [Private Methods]

    #endregion
    #region [Handlers]

    private static IResult HandleSendConfirmationEmail(EmailConfirmationRequest emailConfirmationRequest,
        ILogger<Program> logger, IOptions<SmtpSettings> smtpSettings)
    {
        try
        {
            EmailConfirmationNotification emailConfirmationNotification = new EmailConfirmationNotification()
                .WithTargetEmail(emailConfirmationRequest.TargetEmail)
                .WithConfirmationUrl(emailConfirmationRequest.ConfirmationUrl)
                .WithUserName(emailConfirmationRequest.UserName);
            
            emailConfirmationNotification.Send(smtpSettings.Value);
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