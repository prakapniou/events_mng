namespace EventsManager.Application.Interfaces;

public interface IMessageBroker
{
    public void SendMessage<TMessage>(TMessage message);
}
