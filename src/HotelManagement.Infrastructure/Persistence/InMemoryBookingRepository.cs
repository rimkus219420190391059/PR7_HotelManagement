using System.Collections.Concurrent;
using HotelManagement.Core.Entities;
using HotelManagement.Core.Interfaces;

namespace HotelManagement.Infrastructure.Persistence;

public sealed class InMemoryBookingRepository : IBookingRepository
{
    private readonly ConcurrentDictionary<int, Booking> _bookings = new();

    public Task AddAsync(Booking booking, CancellationToken cancellationToken = default)
    {
        if (!_bookings.TryAdd(booking.Id, booking))
            throw new InvalidOperationException("Не удалось сохранить бронирование с повторяющимся ID.");
        return Task.CompletedTask;
    }

    public Task<bool> HasOverlapAsync(int roomId, DateTime checkIn, DateTime checkOut, CancellationToken cancellationToken = default)
    {
        var exists = _bookings.Values.Any(x =>
            x.RoomId == roomId &&
            x.Status == BookingStatus.Confirmed &&
            checkIn < x.CheckOut && checkOut > x.CheckIn);
        return Task.FromResult(exists);
    }
}
