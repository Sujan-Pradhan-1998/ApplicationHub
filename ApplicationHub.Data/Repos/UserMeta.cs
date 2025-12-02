using ApplicationHub.Api;

namespace ApplicationHub.Data.Repos;

public class UserMeta : IUserMeta
{
    public Guid? Id { get; set; }
    public string? Email { get; set; }
    public string? UserName { get; set; }
    public string? CurrentCompany { get; set; }
}