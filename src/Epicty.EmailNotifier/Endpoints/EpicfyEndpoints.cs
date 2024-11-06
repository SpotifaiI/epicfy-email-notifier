using System.Text;
using System.Text.Json;

using Epicty.EmailNotifier.Models;

using Microsoft.Extensions.Options;

namespace Epicty.EmailNotifier.Endpoints;

internal static class EpicfyEndpoints
{
    internal static IEndpointRouteBuilder AddEpicfyEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var enpicfyEndpoints = endpoints.MapGroup("api/v1/notifications/").WithTags("Send Email Endpoints!")
        .WithOpenApi(x =>
        {
            x.Summary = "Send notification of new registered idea!";
            return x;
        });

        return enpicfyEndpoints;
    }

    #region [Handlers]
    static IResult HandleSendEmailNotification(SendEmail sendEmailDto, ILogger<Program> logger, IOptions<SmtpSettings> smtpSettings)
    {
        try
        {
            var emailNotification = new EmailNotification()
                .WithTargetEmail(sendEmailDto.TargetEmail.ToString())
                .WithUserName(sendEmailDto.CreatedBy)
                .WithIdeaTitle(sendEmailDto.Idea.Title)
                .WithIdeaDescription(sendEmailDto.Idea.Description)
                .WithCreatedAt(sendEmailDto.Idea.CreatedAt);

            emailNotification.Send(smtpSettings.Value);
            logger.LogInformation($"Notificação enviada com sucesso para: {sendEmailDto.TargetEmail}");

            return Results.Ok();
        }
        catch (Exception error)
        {
            logger.LogError($"Erro ao enviar notificação: {error}");
            return Results.BadRequest($"Erro ao enviar notificação: {error}");
        }
    }
    #endregion

    #region [Private Methods]
    static async Task<T> ReadBodyAs<T>(HttpContext context)
    {
        context.Request.EnableBuffering();
        using var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true);

        var bodyString = await reader.ReadToEndAsync();
        context.Request.Body.Position = 0;
        return JsonSerializer.Deserialize<T>(bodyString)!;
    }
    #endregion
}