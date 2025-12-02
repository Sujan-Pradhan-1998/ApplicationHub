using ApplicationHub.Api.Controllers;
using ApplicationHub.Modules.Constants;
using Microsoft.Extensions.Configuration;
using Moq;

namespace ApplicationHub.Tests.Controllers
{
    public class RootControllerTests
    {
        [Fact]
        public void Get_ReturnsConfiguredVersion()
        {
            // Arrange
            var expectedVersion = "1.2.3";
            var mockConfig = new Mock<IConfiguration>();
            mockConfig
                .Setup(cfg => cfg[ApplicationHubConstants.Version])
                .Returns(expectedVersion);

            var controller = new RootController(mockConfig.Object);

            // Act
            var result = controller.Get();

            // Assert
            Assert.Equal(expectedVersion, result);
        }
    }
}