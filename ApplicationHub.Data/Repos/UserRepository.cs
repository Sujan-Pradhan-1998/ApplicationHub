using ApplicationHub.Data.Repos.IRepos;
using ApplicationHub.Modules.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApplicationHub.Data.Repos;

public class UserRepository(AppDbContext context) : IUserRepository
{
    public async Task<User?> GetUserId(Guid userId)
    {
        return await context.Users.FirstOrDefaultAsync(x => x.Id == userId);
    }
}