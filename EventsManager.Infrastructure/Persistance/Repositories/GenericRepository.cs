namespace EventsManager.Infrastructure.Persistance.Repositories;

public abstract class GenericRepository<TEntity>:IGenericRepository<TEntity> where TEntity : Entity
{
    private readonly AppDbContext _context;
    private readonly DbSet<TEntity> _dbSet;

    public GenericRepository(AppDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>();
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllByAsync(Expression<Func<TEntity, bool>>? expression = null)
    {
        IQueryable<TEntity> query = _dbSet;
        query = expression is null ? query : query.Where(expression);
        var result = await query.AsSplitQuery().ToListAsync();
        return result;
    }

    public virtual async Task<TEntity> GetOneByAsync(
        Expression<Func<TEntity, bool>>? expression = null)
    {
        IQueryable<TEntity> query = _dbSet;
        query = expression is null ? query : query.Where(expression); 
        var result = await query.AsSplitQuery().FirstOrDefaultAsync();

        return result!;
    }

    public virtual async Task<TEntity> CreateAsync(TEntity model)
    {
        var result = _dbSet.Entry(model);
        result.State = EntityState.Added;
        await SaveDbChangesAsync();
        return result.Entity;
    }

    public virtual async Task<TEntity> UpdateAsync(TEntity model)
    {
        var result = _dbSet.Entry(model);
        result.State = EntityState.Modified;
        await SaveDbChangesAsync();
        return result.Entity;
    }

    public async Task<bool> DeleteByAsync(Expression<Func<TEntity, bool>> expression)
    {
        var model = await _dbSet.FirstOrDefaultAsync(expression);
        var result = _dbSet.Remove(model!) is not null;
        await SaveDbChangesAsync();
        return result;
    }

    public async Task<bool> IsContains(Expression<Func<TEntity, bool>>? expression = null)
    {
        var result = await _dbSet.AnyAsync(expression!);
        return result;
    }

    public string GetModelType() => typeof(TEntity).Name;
        
    private async Task SaveDbChangesAsync()
    {
        var result = await _context.SaveChangesAsync();
        if (result is 0) throw new InvalidOperationException("Changes saving is failed");
    }
}
