using System.ComponentModel.DataAnnotations;

namespace Epicty.EmailNotifier.Models;

public abstract record NewIdeaRequest(string CreatedBy, Idea Idea, [EmailAddress] string TargetEmail);

public abstract record Idea(string Title, string Description, DateTime CreatedAt);