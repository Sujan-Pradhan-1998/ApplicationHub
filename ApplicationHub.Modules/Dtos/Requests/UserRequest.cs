namespace ApplicationHub.Modules.Dtos.Requests;

public class UserRequest
{
    public Guid? Id { get; set; }
    public required string FirstName { get; set; }
    public string? MiddleName { get; set; }
    public required string LastName { get; set; }
    public bool IsAdmin { get; set; }
    public string? CurrentCompany { get; set; }
    public required string Email { get; set; }
    public required string Password { get; set; }
}