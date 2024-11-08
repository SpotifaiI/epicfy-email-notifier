using System.ComponentModel.DataAnnotations;

namespace Epicty.EmailNotifier.Models;

public record NewIdeaRequest(string CreatedBy, Idea Idea, [EmailAddress] string TargetEmail);

public record Idea(string Title, string Description, DateTime CreatedAt);