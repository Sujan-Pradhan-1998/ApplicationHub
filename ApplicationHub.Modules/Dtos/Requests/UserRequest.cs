namespace ApplicationHub.Modules.Dtos.Requests;

public class UserRequest
{
    public required string FirstName { get; set; }
    public string? MiddleName { get; set; }
    public required string LastName { get; set; }
    public string? CurrentCompany { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
}