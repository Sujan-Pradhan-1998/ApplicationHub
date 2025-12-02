using ApplicationHub.Data;
using ApplicationHub.Data.Repos.IRepos;
using ApplicationHub.Data.Services.IServices;
using ApplicationHub.Modules.Dtos.Requests;
using ApplicationHub.Modules.Entities;
using ApplicationHub.Modules.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace ApplicationHub.Tests.Data
{
    public class ServiceRegistrationTests
    {
        [Fact]
        public void ConfigureServices_RegistersAllServices()
        {
            var services = new ServiceCollection();

            // Add logging to resolve ILogger<T>
            services.AddLogging();

            // Mock IDbOption
            var mockDbOption = new Mock<IDbOption>();
            services.AddSingleton(mockDbOption.Object);

            // Register AppDbContext
            services.AddDbContext<AppDbContext>();

            // Register repositories, services, validators
            services.ConfigureServices();

            var provider = services.BuildServiceProvider();

            // Repositories
            var userRepo = provider.GetService<IUserRepository>();
            var appFormRepo = provider.GetService<IApplicationFormRepository>();
            Assert.NotNull(userRepo);
            Assert.NotNull(appFormRepo);

            // Services
            var userService = provider.GetService<IUserService>();
            var appFormService = provider.GetService<IApplicationFormService>();
            Assert.NotNull(userService);
            Assert.NotNull(appFormService);

            // Validators
            var userValidator = provider.GetService<IValidator<UserRequest>>();
            var loginValidator = provider.GetService<IValidator<LoginRequest>>();
            var appFormValidator = provider.GetService<IValidator<ApplicationFormRequest>>();
            var appFormUpdateValidator = provider.GetService<IValidator<ApplicationFormUpdateRequest>>();
            var paginatedAppFormValidator = provider.GetService<IValidator<PaginatedRequest<ApplicationForm, PagedApplicationFormRequest>>>();

            Assert.NotNull(userValidator);
            Assert.NotNull(loginValidator);
            Assert.NotNull(appFormValidator);
            Assert.NotNull(appFormUpdateValidator);
            Assert.NotNull(paginatedAppFormValidator);

        }
    }
}
