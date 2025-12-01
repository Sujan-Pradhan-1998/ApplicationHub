using ApplicationHub.Modules.Entities;

namespace ApplicationHub.Data.Repos.IRepos;

public interface IUserRepository
{
    public Task<User?> GetUserId(Guid userId);
}