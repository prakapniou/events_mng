namespace EventsManager.Application.Interfaces;

public interface IEventService
{
    public Task<IEnumerable<EventDTO>> GetAsync();

    public Task<EventDTO> GetByIdAsync(Guid id);

    public Task<EventDTO> CreateAsync(EventDTO eventDTO);

    public Task<EventDTO> UpdateAsync(Guid id, EventDTO eventDTO);

    public Task<bool> DeleteAsync(Guid id);
}
