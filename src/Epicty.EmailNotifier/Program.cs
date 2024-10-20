using Epicty.EmailNotifier;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors();

var app = builder.Build();

const string uri = "api/v1/notifications/";
app.MapPost(uri, HandleSendEmailNotification);
app.UseCors(options => options
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
);

app.Run();

#region [endpoints handlers]

static IResult HandleSendEmailNotification(SendEmailDto sendEmailDto)
{
    var emailNotification = new EmailNotification()
        .WithTargetEmail(sendEmailDto.TargetEmail.ToString())
        .WithUserName(sendEmailDto.CreatedBy)
        .WithIdeaTitle(sendEmailDto.Idea.Title)
        .WithIdeaDescription(sendEmailDto.Idea.Description)
        .WithCreatedAt(sendEmailDto.Idea.CreatedAt);

    emailNotification.Send();
    return Results.Ok();
}

#endregion