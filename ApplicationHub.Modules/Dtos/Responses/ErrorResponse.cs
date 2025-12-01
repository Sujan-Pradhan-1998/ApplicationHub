namespace ApplicationHub.Modules.Dtos.Responses;

public class ErrorResponse
{
    public IList<string> Errors { get; } = new List<string>();
}
