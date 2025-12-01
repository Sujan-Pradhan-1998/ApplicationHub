using System.Text.Json.Serialization;
using ApplicationHub.Modules.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ApplicationHub.Modules.Dtos.Requests;

public class ApplicationFormRequest
{
    [BindNever]
    [JsonIgnore] public Guid UserId { get; set; }
    public DateOnly AppliedOn { get; set; }
    public required string Company { get; set; }
    public required string Position { get; set; }
    public string? Description { get; set; }
    public required ApplicationFormStatusEnum FormStatus { get; set; }
}