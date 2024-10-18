using Epicty.EmailNotifier;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

const string uri = "api/v1/notifications/";
app.MapPost(uri, HandleSendEmailNotification);
app.Run();

#region [endpoints handlers]

static IResult HandleSendEmailNotification(SendEmailDto sendEmailDto)
{
    var emailNotification = new EmailNotification()
        .WithUserName(sendEmailDto.CreatedBy)
        .WithIdeaTitle(sendEmailDto.Idea.Title)
        .WithIdeaDescription(sendEmailDto.Idea.Description)
        .WithCreatedAt(sendEmailDto.Idea.CreatedAt)
        .WithTargetEmail(sendEmailDto.TargetEmail.ToString());

    emailNotification.Send();
    return Results.Ok();
}

#endregion