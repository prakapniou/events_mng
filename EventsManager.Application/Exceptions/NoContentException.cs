namespace EventsManager.Application.Exceptions;

public sealed class NoContentException:Exception
{
    public NoContentException(string message) : base(message) { }
}
