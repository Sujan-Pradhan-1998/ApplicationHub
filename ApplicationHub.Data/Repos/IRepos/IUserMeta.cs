namespace ApplicationHub.Api;

public interface IUserMeta
{
    public Guid? Id { get; set; }
    public string? Email { get; set; }
    public string? CurrentCompany { get; set; }
    public string? UserName { get; set; }
}