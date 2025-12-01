using System.ComponentModel.DataAnnotations;
using ApplicationHub.Modules.Enums;

namespace ApplicationHub.Modules.Entities;

public class ApplicationForm
{
    [Key] public required Guid Id { get; set; }
    public required Guid UserId { get; set; }
    public required string Company { get; set; }
    public required string Position { get; set; }
    public string? Description { get; set; }
    public required ApplicationFormStatusEnum FormStatus { get; set; }
    public DateOnly AppliedOn { get; set; }
    public DateTime CreatedOn { get; set; }
}