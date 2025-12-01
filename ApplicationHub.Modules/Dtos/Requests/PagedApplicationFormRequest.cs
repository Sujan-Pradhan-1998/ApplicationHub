using System.Text.Json.Serialization;
using ApplicationHub.Modules.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ApplicationHub.Modules.Dtos.Requests;

public class PagedApplicationFormRequest
{
    [BindNever]
    [JsonIgnore] public Guid UserId { get; set; }
    public DateTime? AppliedOn { get; set; }
    public string? Company { get; set; }
    public string? Position { get; set; }
    public ApplicationFormStatusEnum? FormStatus { get; set; }
}