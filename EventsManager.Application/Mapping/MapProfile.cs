namespace EventsManager.Application.Mapping;

public sealed class MapProfile:Profile
{
    public MapProfile()
    {
        CreateMap<Event, EventDTO>().ReverseMap();
        CreateMap<Speaker, SpeakerDTO>().ReverseMap();
        CreateMap<Sponsor, SponsorDTO>().ReverseMap();
    }
}
