using ApplicationHub.Data.Repos.IRepos;
using ApplicationHub.Modules.Entities;
using DocumentFormat.OpenXml.EMMA;
using Microsoft.EntityFrameworkCore;

namespace ApplicationHub.Data.Repos;

public class UserRepository(AppDbContext context) : IUserRepository
{
    public async Task<User?> GetUserById(Guid userId)
    {
        return await context.Users.FirstOrDefaultAsync(x => x.Id == userId);
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        return await context.Users.FirstOrDefaultAsync(x => x.Email == email.ToLower());
    }

    public async Task<User> AddUser(User user)
    {
        user.Email = user.Email.ToLower();
        var newUser = await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
        return newUser.Entity;
    }

    public async Task<bool> UpdateLastLogin(Guid userId)
    {
        var user = await GetUserById(userId);
        if (user is null) return false;

        user.LastLoginTime = DateTime.UtcNow;
        context.Users.Update(user);
        await context.SaveChangesAsync();
        return true;
    }
}