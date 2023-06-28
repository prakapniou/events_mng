namespace EventsManager.Domain.Aggregates.EventAggregate;

public interface IEventRepository : IGenericRepository<Event>
{
    public void SetValues(Event currentState, Event editState);
}
