namespace EventsManager.Application.Services;

public sealed class SpeakerService : GenericService<Speaker, SpeakerDTO>, ISpeakerService
{
    private readonly ISpeakerRepository _repo;
    private readonly IMapper _mapper;
    private readonly ILogger<SpeakerService> _logger;
    private readonly string _entityType;

    public SpeakerService(
        ISpeakerRepository repo,
        IMapper mapper, 
        ILogger<SpeakerService> logger,
        IValidator<SpeakerDTO> validator)
        : base(repo, mapper, logger, validator)
    {
        _repo=repo;
        _mapper=mapper;
        _logger=logger;
        _entityType=_repo.GetEntityType();
    }

    public override async Task<IEnumerable<SpeakerDTO>> GetAllAsync()
    {
        if (!await _repo.IsContains()) throw new NoContentException($"'{_entityType}' collection not found");

        var models = await _repo.GetAllByAsync();
        _logger.LogInformation("'{ModelType}' collection loaded successfully", _entityType);
        var dtos = _mapper.Map<IEnumerable<SpeakerDTO>>(models);
        return dtos.OrderBy(_=>_.Name);
    }
}
