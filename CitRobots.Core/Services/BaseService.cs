public abstract class BaseService<T> : IBaseService<T> where T : BaseEntity
{
    protected readonly IBaseRepository<T> _repository;
    protected readonly ILogger<BaseService<T>> _logger;

    protected BaseService(IBaseRepository<T> repository, ILogger<BaseService<T>> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public virtual async Task<T> GetByIdAsync(int id)
    {
        try
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null)
                throw new NotFoundException($"Entity of type {typeof(T).Name} with id {id} not found");
            return entity;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting {typeof(T).Name} by id: {id}");
            throw;
        }
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        try
        {
            return await _repository.GetAllAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error getting all {typeof(T).Name}s");
            throw;
        }
    }

    public virtual async Task<T> CreateAsync(T entity)
    {
        try
        {
            return await _repository.AddAsync(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error creating {typeof(T).Name}");
            throw;
        }
    }

    public virtual async Task UpdateAsync(T entity)
    {
        try
        {
            await _repository.UpdateAsync(entity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error updating {typeof(T).Name}");
            throw;
        }
    }

    public virtual async Task DeleteAsync(int id)
    {
        try
        {
            await _repository.DeleteAsync(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting {typeof(T).Name}");
            throw;
        }
    }
}