using Epicty.EmailNotifier.Endpoints;
using Epicty.EmailNotifier.Models;

WebApplicationBuilder? builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));

WebApplication? app = builder.Build();

app.UseHttpsRedirection();
app.UseCors(options => options
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader()
);
app.UseSwagger();
app.AddEpicfyEndpoints();
app.UseSwaggerUI(x => x.SwaggerEndpoint("/swagger/v1/swagger.json", "Epicfy Email Notifier"));
app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();
// app.Use(async (context, next) =>
// {
//     var result = await ReadBodyAs<SendEmail>(context);
//     var errors = EmailNotification.Validate(result);

//     if (errors.Count > 0) await context.Response.WriteAsJsonAsync(new ErrorResponse(statusCode: 400, errors));
//     await next.Invoke();
// });

app.Run();