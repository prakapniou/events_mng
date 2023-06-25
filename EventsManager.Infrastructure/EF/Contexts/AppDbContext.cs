namespace EventsManager.Infrastructure.EF.Contexts;

public sealed class AppDbContext : DbContext
{
    public DbSet<Speaker> Speakers { get; set; }
    public DbSet<Sponsor> Sponsors { get; set; }
    public DbSet<Event> Events { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .ApplyConfiguration(new SpeakerModelConfig())
            .ApplyConfiguration(new SponsorModelConfig())
            .ApplyConfiguration(new EventModelConfig());
    }
}
