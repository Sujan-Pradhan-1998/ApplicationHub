using ApplicationHub.Modules.Dtos.Requests;
using ApplicationHub.Modules.Validators;
using FluentValidation.TestHelper;

namespace ApplicationHub.Tests.Validators
{
    public class UserRequestValidatorTests
    {
        private readonly UserRequestValidator _validator = new();

        [Fact]
        public void Should_Have_Error_When_Email_Is_Empty()
        {
            var model = new UserRequest
            {
                Email = "",
                FirstName = "John",
                LastName = "Doe",
                Password = "Password123"
            };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public void Should_Have_Error_When_Email_Is_Invalid()
        {
            var model = new UserRequest
            {
                Email = "invalidemail",
                FirstName = "John",
                LastName = "Doe",
                Password = "Password123"
            };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public void Should_Have_Error_When_FirstName_Is_Empty()
        {
            var model = new UserRequest
            {
                Email = "test@test.com",
                FirstName = "",
                LastName = "Doe",
                Password = "Password123"
            };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.FirstName);
        }

        [Fact]
        public void Should_Have_Error_When_LastName_Is_Empty()
        {
            var model = new UserRequest
            {
                LastName = "", FirstName = "John",
                Password = "Password123",
                Email = "john.doe@gmail.com",
            };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.LastName);
        }

        [Fact]
        public void Should_Have_Error_When_Password_Is_Empty()
        {
            var model = new UserRequest
            {
                Password = "", 
                FirstName = "John",
                LastName = "Doe",
                Email = "test@test.com",
            };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public void Should_Not_Have_Error_When_All_Properties_Are_Valid()
        {
            var model = new UserRequest
            {
                Email = "test@example.com",
                FirstName = "John",
                LastName = "Doe",
                Password = "Password123"
            };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}