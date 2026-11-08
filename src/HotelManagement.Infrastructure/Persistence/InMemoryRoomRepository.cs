using System.Collections.Concurrent;
using HotelManagement.Core.Entities;
using HotelManagement.Core.Interfaces;

namespace HotelManagement.Infrastructure.Persistence;

public sealed class InMemoryRoomRepository : IRoomRepository
{
    private readonly ConcurrentDictionary<int, Room> _rooms = new();

    public InMemoryRoomRepository()
    {
        _rooms.TryAdd(101, new Room(101, "101", 2, 4500m, RoomStatus.Clean));
        _rooms.TryAdd(102, new Room(102, "102", 2, 5200m, RoomStatus.Dirty));
        _rooms.TryAdd(201, new Room(201, "201", 3, 7000m, RoomStatus.Clean));
        _rooms.TryAdd(301, new Room(301, "301", 2, 6000m, RoomStatus.Occupied));
    }

    public Task<IReadOnlyCollection<Room>> GetAllAsync(CancellationToken cancellationToken = default) =>
        Task.FromResult<IReadOnlyCollection<Room>>(_rooms.Values.OrderBy(x => x.Id).ToArray());

    public Task<Room?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        _rooms.TryGetValue(id, out var room);
        return Task.FromResult(room);
    }

    public Task UpdateAsync(Room room, CancellationToken cancellationToken = default)
    {
        _rooms[room.Id] = room;
        return Task.CompletedTask;
    }
}
