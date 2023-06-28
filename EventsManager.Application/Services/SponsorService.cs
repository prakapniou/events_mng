namespace EventsManager.Application.Services;

public sealed class SponsorService:GenericService<Sponsor,SponsorDTO>, ISponsorService
{
    private readonly ISponsorRepository _repo;
    private readonly IMapper _mapper;
    private readonly ILogger<SponsorService> _logger;
    private readonly string _entityType;

    public SponsorService(
        ISponsorRepository repo,
        IMapper mapper,
        ILogger<SponsorService> logger,
        IValidator<SponsorDTO> validator)
        : base(repo, mapper, logger, validator)
    {
        _repo=repo;
        _mapper=mapper;
        _logger=logger;
        _entityType=_repo.GetEntityType();
    }

    public override async Task<IEnumerable<SponsorDTO>> GetAllAsync()
    {
        if (!await _repo.IsContains()) throw new NoContentException($"'{_entityType}' collection not found");

        var models = await _repo.GetAllByAsync();
        _logger.LogInformation("'{ModelType}' collection loaded successfully", _entityType);
        var dtos = _mapper.Map<IEnumerable<SponsorDTO>>(models);
        return dtos.OrderBy(_ => _.Name);
    }
}
