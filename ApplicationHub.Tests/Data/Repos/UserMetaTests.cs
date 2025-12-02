using ApplicationHub.Data.Repos;

namespace ApplicationHub.Tests.Data.Repos
{
    public class UserMetaTests
    {
        [Fact]
        public void CanSetAndGetProperties()
        {
            var userMeta = new UserMeta
            {
                Id = Guid.NewGuid(),
                Email = "test@example.com",
                UserName = "testuser",
                CurrentCompany = "ExampleCorp"
            };

            Assert.NotNull(userMeta.Id);
            Assert.Equal("test@example.com", userMeta.Email);
            Assert.Equal("testuser", userMeta.UserName);
            Assert.Equal("ExampleCorp", userMeta.CurrentCompany);
        }

        [Fact]
        public void PropertiesCanBeNull()
        {
            var userMeta = new UserMeta();

            Assert.Null(userMeta.Id);
            Assert.Null(userMeta.Email);
            Assert.Null(userMeta.UserName);
            Assert.Null(userMeta.CurrentCompany);
        }
    }
}