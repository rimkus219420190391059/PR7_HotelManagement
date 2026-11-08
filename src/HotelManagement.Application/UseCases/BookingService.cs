using HotelManagement.Application.DTOs;
using HotelManagement.Application.Exceptions;
using HotelManagement.Core.Entities;
using HotelManagement.Core.Interfaces;

namespace HotelManagement.Application.UseCases;

public sealed class BookingService(IBookingRepository bookings, IRoomRepository rooms)
{
    public async Task<int> CreateAsync(CreateBookingRequest request, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(request.GuestName))
            throw new BusinessRuleException("Необходимо указать ФИО гостя.");
        if (request.CheckOut <= request.CheckIn)
            throw new BusinessRuleException("Дата выезда должна быть позже даты заезда.");

        var room = await rooms.GetByIdAsync(request.RoomId, ct)
            ?? throw new NotFoundException($"Номер с ID {request.RoomId} не найден.");
        if (room.Status == RoomStatus.Occupied)
            throw new BusinessRuleException("Номер занят.");
        if (await bookings.HasOverlapAsync(request.RoomId, request.CheckIn, request.CheckOut, ct))
            throw new BusinessRuleException("На выбранный период номер уже забронирован.");

        var guest = new Guest(Random.Shared.Next(1, int.MaxValue), request.GuestName.Trim());
        var booking = new Booking(Random.Shared.Next(1, int.MaxValue), room.Id, guest, request.CheckIn, request.CheckOut);
        await bookings.AddAsync(booking, ct);
        return booking.Id;
    }
}
