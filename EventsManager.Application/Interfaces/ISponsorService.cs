namespace EventsManager.Application.Interfaces;

public interface ISponsorService
{
    public Task<IEnumerable<SponsorDTO>> GetAsync();

    public Task<SponsorDTO> GetByIdAsync(Guid id);

    public Task<SponsorDTO> CreateAsync(SponsorDTO sponsorDTO);

    public Task<SponsorDTO> UpdateAsync(Guid id, SponsorDTO sponsorDTO);

    public Task<bool> DeleteAsync(Guid id);
}
