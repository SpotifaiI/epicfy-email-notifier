using Epicty.EmailNotifier;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();

var app = builder.Build();

const string uri = "api/v1/notifications/";
app.MapPost(uri, HandleSendEmailNotification).WithOpenApi(x =>
{
    x.Summary = "Send notification of new registered idea!";
    return x;
});

app.UseHttpsRedirection();
app.UseCors(options => options
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
);

app.UseSwagger();
app.UseSwaggerUI(x => x.SwaggerEndpoint("/swagger/v1/swagger.json", "Epicfy Email Notifier"));
app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();

app.Run();

#region [methods]

static IResult HandleSendEmailNotification(SendEmailDto sendEmailDto, ILogger<Program> logger)
{
    try
    {
        var emailNotification = new EmailNotification()
            .WithTargetEmail(sendEmailDto.TargetEmail.ToString())
            .WithUserName(sendEmailDto.CreatedBy)
            .WithIdeaTitle(sendEmailDto.Idea.Title)
            .WithIdeaDescription(sendEmailDto.Idea.Description)
            .WithCreatedAt(sendEmailDto.Idea.CreatedAt);

        logger.LogInformation($"Notificação enviada com sucesso para: {sendEmailDto.TargetEmail}");
        emailNotification.Send();

        return Results.Ok();
    }
    catch (Exception error)
    {
        logger.LogError($"Erro ao enviar notificação: {error}");
        return Results.BadRequest($"Erro ao enviar notificação: {error}");
    }
}

#endregion