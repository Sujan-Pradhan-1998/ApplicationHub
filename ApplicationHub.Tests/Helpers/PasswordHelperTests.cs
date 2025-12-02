using ApplicationHub.Modules.Helpers;

namespace ApplicationHub.Tests.Helpers
{
    public class PasswordHelperTests
    {
        [Fact]
        public void CreateHash_ShouldReturnSameHash_ForSamePasswordAndEmail()
        {
            var email = "test@example.com";
            var password = "MySecurePassword123!";

            var hash1 = PasswordHelper.CreateHash(password, email);
            var hash2 = PasswordHelper.CreateHash(password, email);

            Assert.Equal(hash1, hash2);
        }

        [Fact]
        public void Validate_ShouldReturnTrue_ForCorrectPassword()
        {
            var email = "user@example.com";
            var password = "Password123!";

            var hash = PasswordHelper.CreateHash(password, email);

            Assert.True(PasswordHelper.Validate(email, password, hash));
        }

        [Fact]
        public void Validate_ShouldReturnFalse_ForIncorrectPassword()
        {
            var email = "user@example.com";
            var password = "Password123!";
            var wrongPassword = "WrongPassword";

            var hash = PasswordHelper.CreateHash(password, email);

            Assert.False(PasswordHelper.Validate(email, wrongPassword, hash));
        }
    }
}