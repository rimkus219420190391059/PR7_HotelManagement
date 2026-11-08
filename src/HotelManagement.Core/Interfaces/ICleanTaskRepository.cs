using HotelManagement.Core.Entities;

namespace HotelManagement.Core.Interfaces;

public interface ICleanTaskRepository
{
    Task AddAsync(CleanTask task, CancellationToken cancellationToken = default);
    Task<IReadOnlyCollection<CleanTask>> GetAllAsync(CancellationToken cancellationToken = default);
}
