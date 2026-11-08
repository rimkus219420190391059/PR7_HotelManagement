using HotelManagement.Core.Entities;

namespace HotelManagement.Core.Interfaces;

public interface IRoomRepository
{
    Task<IReadOnlyCollection<Room>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Room?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task UpdateAsync(Room room, CancellationToken cancellationToken = default);
}
