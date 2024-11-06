using System.ComponentModel.DataAnnotations;
namespace Epicty.EmailNotifier.Models;

public record SendEmail(string CreatedBy, Idea Idea, [EmailAddress] string TargetEmail);