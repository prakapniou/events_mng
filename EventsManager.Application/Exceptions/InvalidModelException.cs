namespace EventsManager.Application.Exceptions;

public sealed class InvalidModelException:Exception
{
    public InvalidModelException(string message) : base(message) { }
}
