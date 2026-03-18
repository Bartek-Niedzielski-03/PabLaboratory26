using AppCore.Dto;
using AppCore.Entities;
using AppCore.Repositories;

namespace Infrastructure.Memory;

public class MemoryGenericRepository<T> : IGenericRepositoryAsync<T>
    where T : EntityBase
{
    protected readonly Dictionary<Guid, T> _data = new();

    public Task<T?> FindByIdAsync(Guid id)
    {
        _data.TryGetValue(id, out var entity);
        return Task.FromResult(entity);
    }

    public Task<IEnumerable<T>> FindAllAsync()
    {
        IEnumerable<T> result = _data.Values.ToList();
        return Task.FromResult(result);
    }

    public Task<PagedResult<T>> FindPagedAsync(int page, int pageSize)
    {
        if (page < 1)
            throw new ArgumentOutOfRangeException(nameof(page), "Page must be greater than 0.");

        if (pageSize < 1)
            throw new ArgumentOutOfRangeException(nameof(pageSize), "PageSize must be greater than 0.");

        var totalCount = _data.Count;

        var items = _data.Values
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        var result = new PagedResult<T>(items, totalCount, page, pageSize);
        return Task.FromResult(result);
    }

    public Task<T> AddAsync(T entity)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity));

        if (entity.Id == Guid.Empty)
            entity.Id = Guid.NewGuid();

        if (_data.ContainsKey(entity.Id))
            throw new InvalidOperationException($"Entity with id {entity.Id} already exists.");

        _data.Add(entity.Id, entity);
        return Task.FromResult(entity);
    }

    public Task<T> UpdateAsync(T entity)
    {
        if (entity is null)
            throw new ArgumentNullException(nameof(entity));

        if (!_data.ContainsKey(entity.Id))
            throw new KeyNotFoundException($"Entity with id {entity.Id} was not found.");

        _data[entity.Id] = entity;
        return Task.FromResult(entity);
    }

    public Task RemoveByIdAsync(Guid id)
    {
        if (!_data.Remove(id))
            throw new KeyNotFoundException($"Entity with id {id} was not found.");

        return Task.CompletedTask;
    }
}