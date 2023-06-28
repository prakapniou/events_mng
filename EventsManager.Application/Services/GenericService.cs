namespace EventsManager.Application.Services;

public abstract class GenericService<TEntity,TDTO>:IGenericService<TDTO>
    where TEntity : IAggregateRoot
    where TDTO : IBaseDTO
{
    private readonly IGenericRepository<TEntity> _repo;
    private readonly IMapper _mapper;
    private readonly ILogger<GenericService<TEntity, TDTO>> _logger;
    private readonly IValidator<TDTO> _validator;
    private readonly string _entityType;

    public GenericService(
        IGenericRepository<TEntity> repo,
        IMapper mapper,
        ILogger<GenericService<TEntity, TDTO>> logger,
        IValidator<TDTO> validator)
    {
        _repo = repo;
        _mapper = mapper;
        _logger = logger;
        _validator = validator;
        _entityType = _repo.GetEntityType();
    }

    public virtual async Task<IEnumerable<TDTO>> GetAllAsync()
    {
        if (!await _repo.IsContains()) throw new NoContentException($"'{_entityType}' collection not found");

        var models = await _repo.GetAllByAsync();
        _logger.LogInformation("'{ModelType}' collection loaded successfully", _entityType);
        var dtos = _mapper.Map<IEnumerable<TDTO>>(models);
        return dtos;
    }

    public virtual async Task<TDTO> GetByIdAsync(Guid id)
    {
        if (!await _repo.IsContains(_ => _.Id.Equals(id)))
            throw new NoContentException($"'{_entityType}' with Id '{id}' not found");

        var model = await _repo.GetOneByAsync(_ => _.Id.Equals(id));
        _logger.LogInformation("'{ModelType}' with Id '{ModelId}' loaded successfully", _entityType, id);
        var dto = _mapper.Map<TDTO>(model);
        return dto;
    }

    public virtual async Task<TDTO> CreateAsync(TDTO dto)
    {
        var validationResult = await _validator.ValidateAsync(dto);

        if (!validationResult.IsValid)
            throw new InvalidModelException($"Invalid value for '{_entityType}'");

        var model = _mapper.Map<TEntity>(dto);
        var modelResult = await _repo.CreateAsync(model)
            ?? throw new InvalidOperationException($"'{_entityType}' creating failed");

        _logger.LogInformation("'{ModelType}' with Id '{ModelId}' created successfully", _entityType, modelResult!.Id);
        var dtoResult = _mapper.Map<TDTO>(modelResult);
        return dtoResult;
    }

    public virtual async Task<TDTO> UpdateAsync(Guid id, TDTO dto)
    {
        if (!dto.Id.Equals(id))
            throw new InvalidOperationException($"'{_entityType}' Id '{dto.Id}' does not match Id '{id}' from request");

        if (!await _repo.IsContains(_ => _.Id.Equals(id)))
            throw new NoContentException($"'{_entityType}' with Id '{id}' not found");

        var validationResult = await _validator.ValidateAsync(dto);

        if (!validationResult.IsValid)
            throw new InvalidModelException($"Invalid value for '{_entityType}'");

        var model = _mapper.Map<TEntity>(dto);
        var modelResult = await _repo.UpdateAsync(model)
            ?? throw new InvalidOperationException($"'{_entityType}' updating failed");

        var dtoResult = _mapper.Map<TDTO>(modelResult);
        _logger.LogInformation("'{ModelType}' modified successfully", _entityType);
        return dtoResult;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        Expression<Func<TEntity, bool>> expression = _ => _.Id.Equals(id);

        if (!await _repo.IsContains(expression))
            throw new NoContentException
                ($"'{_entityType}' with Id '{id}' not found");

        var result = await _repo.DeleteByAsync(expression);

        if (!result) throw new InvalidOperationException($"'{_entityType}' with Id '{id}' removing failed");

        _logger.LogInformation("'{ModelType}' with Id '{ModelId}' removed successfully", _entityType, id);
        return result;
    }
}
