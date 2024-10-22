using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mail;

using Microsoft.Extensions.Options;

namespace Epicty.EmailNotifier;

record SendEmailDto(string CreatedBy, Idea Idea, [EmailAddress] string TargetEmail);
record Idea(string Title, string Description, DateTime CreatedAt);
public class SmtpSettings
{
    public required string Host { get; set; }
    public required int Port { get; set; }
    public required bool EnableSsl { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    public required string FromEmail { get; set; }
    public required string DisplayName { get; set; }
}

public class EmailNotification()
{
    private string? _targetEmail;
    private string? _ideaTitle;
    private string? _ideaDescription;
    private DateTime _createdAt;
    private string? _createdBy;

    public EmailNotification WithTargetEmail(string targetEmail)
    {
        _targetEmail = targetEmail;
        return this;
    }

    public EmailNotification WithIdeaTitle(string title)
    {
        _ideaTitle = title.Trim();
        return this;
    }

    public EmailNotification WithIdeaDescription(string description)
    {
        _ideaDescription = description.Trim();
        return this;
    }

    public EmailNotification WithCreatedAt(DateTime createdAt)
    {
        _createdAt = createdAt;
        return this;
    }

    public EmailNotification WithUserName(string userName)
    {
        _createdBy = userName;
        return this;
    }

    public void Send(SmtpSettings smtpSettings)
    {
        Validate();
        string subject = $"Nova Ideia Registrada no Epicfy: {_ideaTitle}";
        string body = $@"
        <html>
        <body>
            <p>Olá,</p>
            <p>Uma nova ideia foi registrada no sistema! Confira os detalhes abaixo:</p>
            <ul>
                <li><strong>Título:</strong> {_ideaTitle}</li>
                <li><strong>Descrição:</strong> {_ideaDescription}</li>
                <li><strong>Data de Criação:</strong> {_createdAt:dd/MM/yyyy HH:mm:ss}</li>
                <li><strong>Autor:</strong> {_createdBy}</li>
            </ul>
            <p>Por favor, revise essa ideia e avalie as ações necessárias.</p>
            <br>
            <p>Atenciosamente,<br><strong>Spotfail & Epicfy Ltda.</strong><br></p>
        </body>
        </html>";

        var mensagem = new MailMessage
        {
            From = new MailAddress(smtpSettings.FromEmail, smtpSettings.DisplayName),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        mensagem.To.Add(_targetEmail!);
        var client = new SmtpClient(smtpSettings.Host, smtpSettings.Port)
        {
            Credentials = new NetworkCredential(smtpSettings.Username, smtpSettings.Password),
            EnableSsl = smtpSettings.EnableSsl,
        };

        try
        {
            client.Send(mensagem);
        }
        catch (Exception ex)
        {
            throw new SmtpException($"Erro ao enviar e-mail: {ex.Message}");
        }
    }

    private void Validate()
    {
        var validationErrors = new List<string>();

        if (!MailAddress.TryCreate(_targetEmail, out _)) validationErrors.Add("Valid TargetEmail is required");
        if (string.IsNullOrWhiteSpace(_ideaTitle)) validationErrors.Add("Idea Title is required.");
        if (string.IsNullOrWhiteSpace(_ideaDescription)) validationErrors.Add("Idea Description is required.");
        if (string.IsNullOrWhiteSpace(_createdBy)) validationErrors.Add("UserName is required.");
        if (_createdAt == default) validationErrors.Add("CreatedAt is required.");

        if (validationErrors.Count > 0) throw new InvalidOperationException(string.Join(" ", validationErrors));
    }
}