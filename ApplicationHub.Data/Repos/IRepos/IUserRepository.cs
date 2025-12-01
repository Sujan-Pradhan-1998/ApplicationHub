using ApplicationHub.Modules.Entities;

namespace ApplicationHub.Data.Repos.IRepos;

public interface IUserRepository
{
    public Task<User?> GetUserById(Guid userId);
    public Task<User?> GetUserByEmail(string email);
    public Task<User> AddUser(User user);
    public Task<bool> UpdateLastLogin(Guid userId);
}