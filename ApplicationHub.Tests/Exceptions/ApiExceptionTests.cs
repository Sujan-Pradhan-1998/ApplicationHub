using ApplicationHub.Modules.Exceptions;

namespace ApplicationHub.Tests.Exceptions;

public class ApiExceptionTests
{
    [Fact]
    public void DefaultConstructor_ShouldCreateInstance()
    {
        var exception = new ApiException();
        Assert.NotNull(exception);
    }

    [Fact]
    public void Constructor_WithMessage_ShouldSetMessage()
    {
        var message = "Test message";
        var exception = new ApiException(message);
        Assert.Equal(message, exception.Message);
    }

    [Fact]
    public void Constructor_WithMessageAndInnerException_ShouldSetProperties()
    {
        var message = "Test message";
        var innerException = new Exception("Inner exception");
        var exception = new ApiException(message, innerException);
        Assert.Equal(message, exception.Message);
        Assert.Equal(innerException, exception.InnerException);
    }
}