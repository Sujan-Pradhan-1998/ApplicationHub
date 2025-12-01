using System.ComponentModel.DataAnnotations;

namespace ApplicationHub.Modules.Entities;

public class User
{
    [Key] public required Guid Id { get; set; }
    public required string FirstName { get; set; }
    public string? MiddleName { get; set; }
    public required string LastName { get; set; }
    public string? CurrentCompany { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
    public DateOnly? LastLoginTime { get; set; }
    public DateOnly CreatedOn { get; set; }
}