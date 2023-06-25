namespace EventsManager.Infrastructure.Persistance.Repositories;

public sealed class SponsorRepository:GenericRepository<Sponsor>, ISponsorRepository
{
    private readonly AppDbContext _context;

    public SponsorRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public override async Task<IEnumerable<Sponsor>> GetAllByAsync(Expression<Func<Sponsor, bool>>? expression = null)
    {
        IQueryable<Sponsor> query = _context.Sponsors;
        query = expression is null ? query : query.Where(expression);
        query = query.OrderBy(_ => _.Name).AsNoTracking();
        var result = await query.AsSplitQuery().ToListAsync();
        return result;
    }
}
