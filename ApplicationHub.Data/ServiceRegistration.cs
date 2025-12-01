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

        services.AddScoped(typeof(IPaginationBaseRepository<,>), typeof(PaginationBaseRepository<,>));
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IApplicationFormRepository, ApplicationFormRepository>();

        #endregion

        #region services

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IApplicationFormService, ApplicationFormService>();

        #endregion

        #region validators

        services.AddValidatorsFromAssemblyContaining<UserRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<LoginRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<ApplicationFormRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<PaginatedApplicationFormRequestValidator>();

        #endregion
    }
}