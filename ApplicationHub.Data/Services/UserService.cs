using ApplicationHub.Data.Repos.IRepos;
using ApplicationHub.Data.Services.IServices;
using ApplicationHub.Modules.Entities;

namespace ApplicationHub.Data.Services;

public class UserService(IUserRepository userRepository) : IUserService
{
    public async Task<User?> GetUserId(Guid userId)
    {
        return await userRepository.GetUserId(userId);
    }
}