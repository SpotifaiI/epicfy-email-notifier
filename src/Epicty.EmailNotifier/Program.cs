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

static Task<IResult> HandleSendEmailNotification(SendEmailRequestDto sendEmailRequestDto)
{
    return null!;
}

record SendEmailRequestDto(string TargetEmail, Idea Idea);
record Idea(string Title, string Description, DateTime CreatedAt);

#endregion