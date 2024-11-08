using Epicty.EmailNotifier.Models;

namespace Epicty.EmailNotifier.EmailNotification;

public interface IEmailNotification<T> where T : class
{
    void Send(SmtpSettings smtpSettings);
    List<string> Validate(T item);
}