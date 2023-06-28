namespace EventsManager.Infrastructure.Persistance.Repositories;

public sealed class SponsorRepository:GenericRepository<Sponsor>, ISponsorRepository
{
    public SponsorRepository(AppDbContext context) : base(context) { }
}
