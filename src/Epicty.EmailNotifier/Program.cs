using Epicty.EmailNotifier.Endpoints;
using Epicty.EmailNotifier.Models;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCors();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();
builder.Services.Configure<SmtpSettings>(builder.Configuration.GetSection("SmtpSettings"));

WebApplication app = builder.Build();

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

app.Run();