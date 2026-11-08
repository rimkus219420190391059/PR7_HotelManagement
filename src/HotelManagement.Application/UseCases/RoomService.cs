using HotelManagement.Application.DTOs;
using HotelManagement.Application.Exceptions;
using HotelManagement.Core.Entities;
using HotelManagement.Core.Interfaces;

namespace HotelManagement.Application.UseCases;

public sealed class RoomService(IRoomRepository rooms)
{
    public async Task<IReadOnlyCollection<RoomDto>> GetRoomsAsync(CancellationToken ct = default)
    {
        var items = await rooms.GetAllAsync(ct);
        return items.Select(RoomDto.FromEntity).ToArray();
    }

    public async Task<RoomDto> GetRoomAsync(int id, CancellationToken ct = default)
    {
        var room = await rooms.GetByIdAsync(id, ct)
            ?? throw new NotFoundException($"Номер с ID {id} не найден.");
        return RoomDto.FromEntity(room);
    }

    public async Task<RoomDto> ChangeStatusAsync(int id, RoomStatus status, CancellationToken ct = default)
    {
        var room = await rooms.GetByIdAsync(id, ct)
            ?? throw new NotFoundException($"Номер с ID {id} не найден.");
        if (room.Status == RoomStatus.Occupied && status != RoomStatus.Occupied)
            throw new BusinessRuleException("Нельзя изменить статус занятого номера до выезда гостя.");
        room.SetStatus(status);
        await rooms.UpdateAsync(room, ct);
        return RoomDto.FromEntity(room);
    }
}
