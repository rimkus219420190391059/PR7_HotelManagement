using HotelManagement.Application.Exceptions;
using HotelManagement.Core.Entities;
using HotelManagement.Core.Interfaces;

namespace HotelManagement.Application.UseCases;

public sealed class CleaningService(IRoomRepository rooms, ICleanTaskRepository tasks)
{
    public async Task<int> RegisterCleaningAsync(int roomId, string employeeName, CancellationToken ct = default)
    {
        var room = await rooms.GetByIdAsync(roomId, ct)
            ?? throw new NotFoundException($"Номер с ID {roomId} не найден.");
        if (room.Status == RoomStatus.Occupied)
            throw new BusinessRuleException("Нельзя начать уборку занятого номера.");
        if (string.IsNullOrWhiteSpace(employeeName))
            throw new BusinessRuleException("Необходимо указать имя сотрудника.");

        room.SetStatus(RoomStatus.Cleaning);
        await rooms.UpdateAsync(room, ct);
        var task = new CleanTask(Random.Shared.Next(1, int.MaxValue), roomId, employeeName.Trim());
        await tasks.AddAsync(task, ct);
        return task.Id;
    }
}
