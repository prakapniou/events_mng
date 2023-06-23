namespace EventsManager.Domain.Aggregates.EventAggregate;

public sealed class Event:AppEntity
{
    private readonly DateTime _created;
    private readonly List<Speaker> _speakers;
    private readonly List<Sponsor> _sponsors;

    public string? Topic { get; set; }
    public string? Description { get; set; }
    public string? Schedule { get; set; }
    public string? Address { get; set; }
    public DateTime Spending { get; set; }
    public DateTime Created => _created;
    public IReadOnlyCollection<Speaker> Speakers => _speakers;
    public IReadOnlyCollection<Sponsor> Sponsors => _sponsors;

    public Event()
    {
        _speakers=new();
        _sponsors=new();
        _created=DateTime.UtcNow;
    }

    public Event(
        string name,
        string topic,
        string description,
        string schedule,
        string address,
        DateTime spending,
        List<Speaker> speakers,
        List<Sponsor> sponsors)
    {
        Name = name;
        Topic = topic;
        Description = description;
        Schedule = schedule;
        Address = address;
        Spending = spending;
        _created = DateTime.UtcNow;
        _speakers=speakers;
        _sponsors=sponsors;
    }

    public void UpdateNavigation(ICollection<Speaker> speakers, ICollection<Sponsor> sponsors)
    {
        _speakers.Clear();
        _speakers.AddRange(speakers);
        _sponsors.Clear();
        _sponsors?.AddRange(sponsors);
    }
}
