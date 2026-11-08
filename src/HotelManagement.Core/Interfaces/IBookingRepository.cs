using HotelManagement.Core.Entities;

namespace HotelManagement.Core.Interfaces;

public interface IBookingRepository
{
    Task AddAsync(Booking booking, CancellationToken cancellationToken = default);
    Task<bool> HasOverlapAsync(int roomId, DateTime checkIn, DateTime checkOut, CancellationToken cancellationToken = default);
}
