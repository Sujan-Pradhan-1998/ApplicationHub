using ApplicationHub.Modules.Dtos.Requests;
using ApplicationHub.Modules.Dtos.Responses;
using ApplicationHub.Modules.Entities;

namespace ApplicationHub.Data.Services.IServices;

public interface IUserService
{
    public Task<UserResponse?> GetUserById(Guid userId);
    public Task<UserResponse?> AddUser(UserRequest userRequest);
    public Task<UserResponse?> Login(LoginRequest loginRequest);
}