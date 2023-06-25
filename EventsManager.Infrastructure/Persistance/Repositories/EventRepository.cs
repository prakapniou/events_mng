namespace EventsManager.Infrastructure.Persistance.Repositories;

public sealed class EventRepository : GenericRepository<Event>, IEventRepository
{
    private readonly AppDbContext _context;

    public EventRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public override async Task<IEnumerable<Event>> GetAllByAsync(Expression<Func<Event, bool>>? expression = null)
    {
        IQueryable<Event> query = _context.Events;
        query = expression is null ? query : query.Where(expression);
        query=query.Include(_ => _.Speakers).Include(_=>_.Sponsors);
        query = query.OrderBy(_ => _.Spending).AsNoTracking();
        var result = await query.AsSplitQuery().ToListAsync();
        return result;
    }

    public override async Task<Event> GetOneByAsync(Expression<Func<Event, bool>>? expression = null)
    {
        IQueryable<Event> query = _context.Events;
        query = expression is null ? query : query.Where(expression);
        query=query.Include(_ => _.Speakers).Include(_ => _.Sponsors);
        var result = await query.AsSplitQuery().FirstOrDefaultAsync();

        return result!;
    }

    public void Attach(Event entity)
    {
        _context.Attach(entity);
    }

    public void SetValues(Event currentState, Event editState)
    {
        _context.Events.Entry(currentState).CurrentValues.SetValues(editState);
    }
}
