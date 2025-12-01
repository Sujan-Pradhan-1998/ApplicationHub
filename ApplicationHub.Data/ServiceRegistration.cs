using ApplicationHub.Data.Repos;
using ApplicationHub.Data.Repos.IRepos;
using ApplicationHub.Data.Services;
using ApplicationHub.Data.Services.IServices;
using ApplicationHub.Modules.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace ApplicationHub.Data;

public static class ServiceRegistration
{
    public static void ConfigureServices(this IServiceCollection services)
    {
        #region repos

        services.AddScoped<IUserRepository, UserRepository>();

        #endregion

        #region services

        services.AddScoped<IUserService, UserService>();

        #endregion

        #region validators
        services.AddValidatorsFromAssemblyContaining<UserRequestValidator>();
        #endregion
    }
}