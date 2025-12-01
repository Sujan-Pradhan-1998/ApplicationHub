using ApplicationHub.Data.Repos.IRepos;
using ApplicationHub.Data.Services.IServices;
using ApplicationHub.Modules.Dtos.Requests;
using ApplicationHub.Modules.Dtos.Responses;
using ApplicationHub.Modules.Entities;
using ApplicationHub.Modules.Helpers;
using Mapster;
using Microsoft.Extensions.Logging;

namespace ApplicationHub.Data.Services;

public class UserService(IUserRepository userRepository, ILogger<UserService> logger) : IUserService
{
    public async Task<UserResponse?> GetUserById(Guid userId)
    {
        var user = await userRepository.GetUserById(userId);
        if (user is null) return null;
        return user.Adapt<UserResponse>();
    }

    public async Task<UserResponse?> AddUser(UserRequest userRequest)
    {
        var user = userRequest.Adapt<User>();

        //Update password hash
        user.Password = PasswordHelper.CreateHash(userRequest.Password, user.Email);
        var newUser = await userRepository.AddUser(user);
        return newUser.Adapt<UserResponse>();
    }

    public async Task<UserResponse?> Login(LoginRequest loginRequest)
    {
        var user = await userRepository.GetUserByEmail(loginRequest.Email);
        if (user is null) return null;

        if (!PasswordHelper.Validate(loginRequest.Email, loginRequest.Password, user.Password))
        {
            logger.LogInformation("Invalid login attempt for {Email}", user.Email);
            return null;
        }

        await userRepository.UpdateLastLogin(user.Id);
        return user.Adapt<UserResponse>();
    }
}