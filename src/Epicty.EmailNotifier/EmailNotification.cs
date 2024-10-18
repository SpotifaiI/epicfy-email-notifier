using System.Net;
using System.Net.Mail;

namespace Epicty.EmailNotifier;

record SendEmailDto(string CreatedBy, Idea Idea, MailAddress TargetEmail);
record Idea(string Title, string Description, DateTime CreatedAt);

public class EmailNotification
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

    public void Send()
    {
        Validate();
        string subject = $"Nova Idea Registrada no Epicfy: {_targetEmail}";
        string body = $@"
Olá,

Uma nova ideia foi registrada no sistema. Aqui estão os detalhes:

**Título da Ideia:** {_ideaTitle}  
**Descrição:** {_ideaDescription}  
**Data de Criação:** {_createdAt:dd/MM/yyyy HH:mm:ss}
**Criada por:** {_createdBy}

Por favor, revise a ideia e considere as próximas ações que podem ser tomadas.

Atenciosamente,  
[Seu Nome ou Nome da Sua Empresa]
";

        var client = new SmtpClient("sandbox.smtp.mailtrap.io", 2525)
        {
            Credentials = new NetworkCredential("5919d424bd5f5f", "******3d223"),
            EnableSsl = true
        };

        client.Send("2a202b6687-0c6cbd@inbox.mailtrap.io", _targetEmail!, subject, body);
    }

    private void Validate()
    {
        var validationErrors = new List<string>();

        if (MailAddress.TryCreate(_targetEmail, out _)) validationErrors.Add("Valid TargetEmail is required");
        if (string.IsNullOrWhiteSpace(_ideaTitle)) validationErrors.Add("Idea Title is required.");
        if (string.IsNullOrWhiteSpace(_ideaDescription)) validationErrors.Add("Idea Description is required.");
        if (string.IsNullOrWhiteSpace(_createdBy)) validationErrors.Add("UserName is required.");
        if (_createdAt == default) validationErrors.Add("CreatedAt is required.");

        if (validationErrors.Count > 0) throw new InvalidOperationException(string.Join(" ", validationErrors));
    }
}