namespace EventsManager.Application.Interfaces;

public interface ISpeakerService
{
    public Task<IEnumerable<SpeakerDTO>> GetAsync();

    public Task<SpeakerDTO> GetByIdAsync(Guid id);
    
    public Task<SpeakerDTO> CreateAsync(SpeakerDTO speakerDTO);

    public Task<SpeakerDTO> UpdateAsync(Guid id, SpeakerDTO speakerDTO);

    public Task<bool> DeleteAsync(Guid id);
}
