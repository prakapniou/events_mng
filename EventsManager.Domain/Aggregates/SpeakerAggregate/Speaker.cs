namespace EventsManager.Domain.Aggregates.SpeakerAggregate;

public sealed class Speaker:Entity
{
    public string Name { get; set; }

    public Speaker(string name)
    {
        Name = name;
    }
}
