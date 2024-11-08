using System.ComponentModel.DataAnnotations;

namespace Epicty.EmailNotifier.Models.Requests;

public record EmailConfirmationRequest([Url] string ConfirmationUrl, string TargetEmail, string UserName);