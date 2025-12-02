using ApplicationHub.Modules.Dtos.Requests;
using ApplicationHub.Modules.Entities;
using ApplicationHub.Modules.Validators;
using FluentValidation.TestHelper;

namespace ApplicationHub.Tests.Validators
{
    public class PaginatedApplicationFormRequestValidatorTests
    {
        private readonly PaginatedApplicationFormRequestValidator _validator = new();

        [Fact]
        public void Should_Have_Error_When_UserId_Is_Empty()
        {
            var request = new PaginatedRequest<ApplicationForm, PagedApplicationFormRequest>
            {
                Model = new PagedApplicationFormRequest { UserId = Guid.Empty },
                PageNumber = 1,
                PageSize = 10
            };

            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.Model!.UserId);
        }

        [Fact]
        public void Should_Have_Error_When_PageNumber_Is_Empty()
        {
            var request = new PaginatedRequest<ApplicationForm, PagedApplicationFormRequest>
            {
                Model = new PagedApplicationFormRequest { UserId = Guid.NewGuid() },
                PageNumber = 0,
                PageSize = 10
            };

            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.PageNumber);
        }

        [Fact]
        public void Should_Have_Error_When_PageSize_Is_Empty()
        {
            var request = new PaginatedRequest<ApplicationForm, PagedApplicationFormRequest>
            {
                Model = new PagedApplicationFormRequest { UserId = Guid.NewGuid() },
                PageNumber = 1,
                PageSize = 0
            };

            var result = _validator.TestValidate(request);
            result.ShouldHaveValidationErrorFor(x => x.PageSize);
        }

        [Fact]
        public void Should_Not_Have_Error_When_All_Fields_Are_Valid()
        {
            var request = new PaginatedRequest<ApplicationForm, PagedApplicationFormRequest>
            {
                Model = new PagedApplicationFormRequest { UserId = Guid.NewGuid() },
                PageNumber = 1,
                PageSize = 10
            };

            var result = _validator.TestValidate(request);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}