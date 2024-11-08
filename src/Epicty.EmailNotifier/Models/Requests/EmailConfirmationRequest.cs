namespace Epicty.EmailNotifier.Models.Requests;

public abstract record EmailConfirmationRequest(Uri ConfirmationUrl, string TargetEmail, string UserName);