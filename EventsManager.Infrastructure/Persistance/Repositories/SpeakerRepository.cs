namespace EventsManager.Infrastructure.Persistance.Repositories;

public sealed class SpeakerRepository : GenericRepository<Speaker>, ISpeakerRepository
{
    private readonly AppDbContext _context;

    public SpeakerRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public override async Task<IEnumerable<Speaker>> GetAllByAsync(Expression<Func<Speaker, bool>>? expression = null)
    {
        IQueryable<Speaker> query = _context.Speakers;
        query = expression is null ? query : query.Where(expression);
        query = query.OrderBy(_=>_.Name).AsNoTracking();
        var result = await query.AsSplitQuery().ToListAsync();
        return result;
    }
}
