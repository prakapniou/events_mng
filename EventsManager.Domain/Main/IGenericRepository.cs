namespace EventsManager.Domain.Main;

public interface IGenericRepository<TEntity> where TEntity : IAggregateRoot
{
    public Task<IEnumerable<TEntity>> GetAllByAsync(Expression<Func<TEntity, bool>>? expression = null);

    public Task<TEntity> GetOneByAsync(Expression<Func<TEntity, bool>>? expression = null);

    public Task<TEntity> CreateAsync(TEntity entity);

    public Task<TEntity> UpdateAsync(TEntity entity);

    public Task<bool> DeleteByAsync(Expression<Func<TEntity, bool>> expression);

    public Task<bool> IsContains(Expression<Func<TEntity, bool>>? expression = null);

    public string GetModelType();
}
