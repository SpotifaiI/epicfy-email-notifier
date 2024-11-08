using System.Net;
using System.Net.Mail;
using Epicty.EmailNotifier.Models;

namespace Epicty.EmailNotifier.EmailNotification;

public class NewIdeaEmailNotification() : IEmailNotification<NewIdeaRequest>
{
    private string? _targetEmail;
    private string? _ideaTitle;
    private string? _ideaDescription;
    private DateTime _createdAt;
    private string? _createdBy;

    public NewIdeaEmailNotification WithTargetEmail(string targetEmail)
    {
        _targetEmail = targetEmail;
        return this;
    }

    public NewIdeaEmailNotification WithIdeaTitle(string title)
    {
        _ideaTitle = title.Trim();
        return this;
    }

    public NewIdeaEmailNotification WithIdeaDescription(string description)
    {
        _ideaDescription = description.Trim();
        return this;
    }

    public NewIdeaEmailNotification WithCreatedAt(DateTime createdAt)
    {
        _createdAt = createdAt;
        return this;
    }

    public NewIdeaEmailNotification WithUserName(string userName)
    {
        _createdBy = userName;
        return this;
    }

    public void Send(SmtpSettings smtpSettings)
    {
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

    public List<string> Validate(NewIdeaRequest newIdeaRequest)
    {
        var validationErrors = new List<string>();

        if (!MailAddress.TryCreate(newIdeaRequest.TargetEmail, out _)) validationErrors.Add("Valid TargetEmail is required");
        if (string.IsNullOrWhiteSpace(newIdeaRequest.Idea.Title)) validationErrors.Add("Idea Title is required.");
        if (string.IsNullOrWhiteSpace(newIdeaRequest.Idea.Description)) validationErrors.Add("Idea Description is required.");
        if (string.IsNullOrWhiteSpace(newIdeaRequest.CreatedBy)) validationErrors.Add("CreatedBy is required.");

        return validationErrors;
    }
}