namespace EventsManager.Domain.Aggregates.SponsorAggregate;

public sealed class Sponsor:Entity
{
    public string Name { get; set; }

    public Sponsor(string name)
    {
        Name = name;
    }
}
