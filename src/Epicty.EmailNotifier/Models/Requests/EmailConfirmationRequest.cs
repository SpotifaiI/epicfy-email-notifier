using System.ComponentModel.DataAnnotations;

namespace Epicty.EmailNotifier.Models.Requests;

public abstract record EmailConfirmationRequest([Url] string ConfirmationUrl, string TargetEmail, string UserName);