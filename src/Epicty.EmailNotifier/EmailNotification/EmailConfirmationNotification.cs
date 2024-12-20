using System.Net;
using System.Net.Mail;

using Epicty.EmailNotifier.Models;
using Epicty.EmailNotifier.Models.Requests;

namespace Epicty.EmailNotifier.EmailNotification;

public class EmailConfirmationNotification : IEmailNotification<EmailConfirmationRequest>
{
    private string? _confirmationUrl;
    private string? _targetEmail;
    private string? _userName;

    public void Send(SmtpSettings smtpSettings)
    {
        string subject = "Confirmação de E-mail - Epicfy";
        string body = $@"
            <html>
            <body>
                <p>Olá {_userName},</p>
                <p>Obrigado por se registrar no Epicfy! Para confirmar seu e-mail e ativar sua conta, clique no link abaixo:</p>
                <p><a href='{_confirmationUrl}'>Confirmar E-mail</a></p>
                <p>Este link expira em 24 horas. Caso não tenha feito essa solicitação, ignore este e-mail.</p>
                <br>
                <p>Atenciosamente,<br><strong>Spotfail & Epicfy Ltda.</strong><br></p>
            </body>
            </html>";

        MailMessage mensagem = new()
        {
            From = new MailAddress(smtpSettings.FromEmail, smtpSettings.DisplayName),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        mensagem.To.Add(_targetEmail!);
        SmtpClient client = new(smtpSettings.Host, smtpSettings.Port)
        {
            Credentials = new NetworkCredential(smtpSettings.Username, smtpSettings.Password),
            EnableSsl = smtpSettings.EnableSsl
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

    public List<string> Validate(EmailConfirmationRequest emailConfirmationRequest)
    {
        List<string> validationErrors = [];

        if (!MailAddress.TryCreate(emailConfirmationRequest.TargetEmail, out _))
        {
            validationErrors.Add("Valid TargetEmail is required.");
        }

        if (string.IsNullOrWhiteSpace(emailConfirmationRequest.ConfirmationUrl))
        {
            validationErrors.Add("Confirmation URL is required.");
        }

        if (string.IsNullOrWhiteSpace(emailConfirmationRequest.UserName))
        {
            validationErrors.Add("UserName is required.");
        }

        return validationErrors;
    }

    public EmailConfirmationNotification WithTargetEmail(string targetEmail)
    {
        _targetEmail = targetEmail;
        return this;
    }

    public EmailConfirmationNotification WithConfirmationUrl(string confirmationUrl)
    {
        _confirmationUrl = confirmationUrl;
        return this;
    }

    public EmailConfirmationNotification WithUserName(string userName)
    {
        _userName = userName;
        return this;
    }
}