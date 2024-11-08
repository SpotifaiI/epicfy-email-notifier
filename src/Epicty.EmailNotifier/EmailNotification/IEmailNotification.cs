using Epicty.EmailNotifier.Models;
namespace Epicty.EmailNotifier.EmailNotification;

public interface IEmailNotification<T> where T : class
{
    public void Send(SmtpSettings smtpSettings);
    public List<string> Validate(T item);
}