namespace EventsManager.Infrastructure.Persistance.Repositories;

public sealed class SpeakerRepository : GenericRepository<Speaker>, ISpeakerRepository
{
    public SpeakerRepository(AppDbContext context) : base(context) { }
}
