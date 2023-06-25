namespace EventsManager.Domain.Aggregates.EventAggregate;

public interface IEventRepository : IGenericRepository<Event>
{
    public void Attach(Event model);

    public void SetValues(Event currentState, Event editState);
}
