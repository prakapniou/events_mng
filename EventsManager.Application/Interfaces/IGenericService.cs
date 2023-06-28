namespace EventsManager.Application.Interfaces;

public interface IGenericService<TDTO> where TDTO : IBaseDTO
{
    public Task<IEnumerable<TDTO>> GetAllAsync();

    public Task<TDTO> GetByIdAsync(Guid id);

    public Task<TDTO> CreateAsync(TDTO dto);

    public Task<TDTO> UpdateAsync(Guid id, TDTO dto);

    public Task<bool> DeleteAsync(Guid id);
}
