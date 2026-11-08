using HotelManagement.Core.Entities;

namespace HotelManagement.Application.DTOs;

public sealed record RoomDto(int Id, string Number, int Capacity, decimal PricePerNight, string Status)
{
    public static RoomDto FromEntity(Room room) =>
        new(room.Id, room.Number, room.Capacity, room.PricePerNight, room.Status.ToString());
}
