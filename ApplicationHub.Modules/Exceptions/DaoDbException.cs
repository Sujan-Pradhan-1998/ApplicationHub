namespace ApplicationHub.Modules.Exceptions;
 
public class DaoDbException : Exception
{
    public DaoDbException()
    {
    }

    public DaoDbException(string message) : base(message)
    {
    }

    public DaoDbException(string message, Exception innerException) : base(message, innerException)
    {
    }
}