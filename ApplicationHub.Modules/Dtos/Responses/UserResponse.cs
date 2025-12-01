namespace ApplicationHub.Modules.Dtos.Responses;

public class UserResponse
{
    public Guid Id { get; set; }
    public required string FirstName { get; set; }
    public string? MiddleName { get; set; }
    public required string LastName { get; set; }
    public string? CurrentCompany { get; set; }
    public required string Email { get; set; }
    public DateTime? LastLoginTime { get; set; }
    public DateTime CreatedOn { get; set; }
}