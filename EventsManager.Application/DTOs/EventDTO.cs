namespace EventsManager.Application.DTOs;

public sealed class EventDTO:AppDTO
{
    public string? Topic { get; set; }
    public string? Description { get; set; }
    public string? Schedule { get; set; }
    public string? Address { get; set; }
    public DateTime Spending { get; set; }
    public DateTime Created { get; set; }
    public List<Guid> SpeakerIds { get; set; } = new();
    public List<Guid> SponsorIds { get; set; } = new();
}
