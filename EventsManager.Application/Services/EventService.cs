namespace EventsManager.Application.Services;

public sealed class EventService : GenericService<Event, EventDTO>, IEventService
{
    private readonly IEventRepository _eventRepo;
    private readonly ISpeakerRepository _speakerRepo;
    private readonly ISponsorRepository _sponsorpRepo;
    private readonly IMapper _mapper;
    private readonly ILogger<EventService> _logger;
    private readonly IValidator<EventDTO> _validator;
    private readonly string _entityType;

    public EventService(
        IEventRepository eventRepo,
        ISpeakerRepository speakerRepo,
        ISponsorRepository sponsorpRepo,
        IMapper mapper,
        ILogger<EventService> logger,
        IValidator<EventDTO> validator)
        : base(eventRepo, mapper, logger, validator)
    {
        _eventRepo=eventRepo;
        _speakerRepo=speakerRepo;
        _sponsorpRepo=sponsorpRepo;
        _mapper=mapper;
        _logger=logger;
        _validator=validator;
        _entityType=_eventRepo.GetEntityType();
    }

    public override async Task<IEnumerable<EventDTO>> GetAllAsync()
    {
        var models = await _eventRepo.GetAllByAsync()
            ?? throw new NoContentException($"'{_entityType}' collection not found");

        _logger.LogInformation("'{ModelType}' collection loaded successfully", _entityType);
        var dtos = new List<EventDTO>();

        foreach (var model in models)
        {
            var dto = MapWithIdCollections(model);
            dtos.Add(dto);
        }

        return dtos.OrderBy(_=>_.Spending);
    }

    public override async Task<EventDTO> GetByIdAsync(Guid id)
    {
        if (!await _eventRepo.IsContains(_ => _.Id.Equals(id)))
            throw new NoContentException($"'{_entityType}' with Id '{id}' not found");

        var model = await _eventRepo.GetOneByAsync(_ => _.Id.Equals(id));

        _logger.LogInformation("'{ModelType}' with Id '{ModelId}' loaded successfully", _entityType, id);
        var dto = MapWithIdCollections(model);
        return dto;
    }

    public override async Task<EventDTO> CreateAsync(EventDTO dto)
    {
        var validationResult = await _validator.ValidateAsync(dto);

        if (!validationResult.IsValid)
            throw new InvalidModelException($"Invalid value for '{_entityType}'");

        var model = _mapper.Map<Event>(dto);

        foreach (var speakerId in dto.SpeakerIds!)
        {
            var speakerFromDb = await _speakerRepo.GetOneByAsync(_ => _.Id.Equals(speakerId));
            if (speakerFromDb is not null) model.Speakers!.Add(speakerFromDb);
        }

        foreach (var sponsorId in dto.SponsorIds!)
        {
            var sponsorFromDb = await _sponsorpRepo.GetOneByAsync(_ => _.Id.Equals(sponsorId));
            if (sponsorFromDb is not null) model.Sponsors!.Add(sponsorFromDb);
        }

        var modelResult = await _eventRepo.CreateAsync(model)
            ?? throw new InvalidOperationException($"'{_entityType}' creating failed");

        _logger.LogInformation("'{ModelType}' with Id '{ModelId}' created successfully", _entityType, modelResult!.Id);
        var dtoResult = MapWithIdCollections(modelResult);
        return dtoResult;
    }

    public override async Task<EventDTO> UpdateAsync(Guid id, EventDTO dto)
    {
        if (!dto.Id.Equals(id))
            throw new InvalidOperationException($"'{_entityType}' Id '{dto.Id}' does not match Id '{id}' from request");

        if (!await _eventRepo.IsContains(_ => _.Id.Equals(id)))
            throw new NoContentException($"'{_entityType}' with Id '{id}' not found");

        var validationResult = await _validator.ValidateAsync(dto);

        if (!validationResult.IsValid)
            throw new InvalidModelException($"Invalid value for '{_entityType}'");

        var eventFromDb = await _eventRepo.GetOneByAsync(_ => _.Id.Equals(id));
        var subjectModel = _mapper.Map<Event>(dto);

        _eventRepo.SetValues(eventFromDb, subjectModel);

        eventFromDb.Speakers!.Clear();
        eventFromDb.Sponsors!.Clear();

        foreach (var speakerId in dto.SpeakerIds!.ToList())
        {
            var speakerFromDb = await _speakerRepo.GetOneByAsync(_ => _.Id.Equals(speakerId));

            if (speakerFromDb is not null) eventFromDb.Speakers.Add(speakerFromDb);
            else _eventRepo.Attach(speakerFromDb!);
        }

        foreach (var sponsorId in dto.SponsorIds!.ToList())
        {
            var sponsorFromDb = await _sponsorpRepo.GetOneByAsync(_ => _.Id.Equals(sponsorId));

            if (sponsorFromDb is not null) eventFromDb.Sponsors.Add(sponsorFromDb);
            else _eventRepo.Attach(sponsorFromDb!);
        }

        var result = await _eventRepo.UpdateAsync(eventFromDb)
            ?? throw new InvalidOperationException($"'{_entityType}' updating failed");

        var dtoResult = MapWithIdCollections(result);
        return dtoResult;
    }

    private EventDTO MapWithIdCollections(Event model)
    {
        var dto = _mapper.Map<EventDTO>(model);
        dto.SpeakerIds=model.Speakers!.Select(s => s.Id).ToList();
        dto.SponsorIds=model.Sponsors!.Select(s => s.Id).ToList();
        return dto;
    }
}
