using ApplicationHub.Modules.Exceptions;

namespace ApplicationHub.Tests.Exceptions;

public class DaoDbExceptionTests
{
    [Fact]
    public void ParameterlessConstructor_ShouldCreateException()
    {
        var ex = new DaoDbException();
        Assert.NotNull(ex);
    }

    [Fact]
    public void ConstructorWithMessage_ShouldSetMessage()
    {
        var message = "Test exception message";
        var ex = new DaoDbException(message);
        Assert.Equal(message, ex.Message);
    }

    [Fact]
    public void ConstructorWithMessageAndInnerException_ShouldSetProperties()
    {
        var message = "Test exception message";
        var inner = new Exception("Inner exception");
        var ex = new DaoDbException(message, inner);
        Assert.Equal(message, ex.Message);
        Assert.Equal(inner, ex.InnerException);
    }
}