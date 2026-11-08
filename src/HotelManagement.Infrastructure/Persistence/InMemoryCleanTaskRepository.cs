using System.Collections.Concurrent;
using HotelManagement.Core.Entities;
using HotelManagement.Core.Interfaces;

namespace HotelManagement.Infrastructure.Persistence;

public sealed class InMemoryCleanTaskRepository : ICleanTaskRepository
{
    private readonly ConcurrentDictionary<int, CleanTask> _tasks = new();

    public Task AddAsync(CleanTask task, CancellationToken cancellationToken = default)
    {
        if (!_tasks.TryAdd(task.Id, task))
            throw new InvalidOperationException("Не удалось сохранить задачу уборки.");
        return Task.CompletedTask;
    }

    public Task<IReadOnlyCollection<CleanTask>> GetAllAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyCollection<CleanTask>>(_tasks.Values.OrderByDescending(x => x.CreatedAt).ToArray());
}
