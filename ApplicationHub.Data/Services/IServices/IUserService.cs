using ApplicationHub.Modules.Entities;

namespace ApplicationHub.Data.Services.IServices;

public interface IUserService
{
    public Task<User?> GetUserId(Guid userId);
}