namespace ApplicationHub.Api;

public class UserMeta : IUserMeta
{
    public Guid? Id { get; set; }
    public string? Email { get; set; }
    public string? CurrentCompany { get; set; }
}