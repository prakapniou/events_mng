namespace EventsManager.Domain.Main;

public abstract class Entity:IAggregateRoot
{
    public Guid Id { get; set; }
}
