namespace HotelManagement.Core.Entities;

public enum BookingStatus
{
    Confirmed,
    Cancelled,
    Completed
}

public sealed class Booking
{
    public int Id { get; }
    public int RoomId { get; }
    public Guest Guest { get; }
    public DateTime CheckIn { get; }
    public DateTime CheckOut { get; }
    public BookingStatus Status { get; private set; }

    public Booking(int id, int roomId, Guest guest, DateTime checkIn, DateTime checkOut)
    {
        if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));
        if (roomId <= 0) throw new ArgumentOutOfRangeException(nameof(roomId));
        if (checkOut <= checkIn) throw new ArgumentException("Дата выезда должна быть позже даты заезда.");
        Id = id;
        RoomId = roomId;
        Guest = guest ?? throw new ArgumentNullException(nameof(guest));
        CheckIn = checkIn;
        CheckOut = checkOut;
        Status = BookingStatus.Confirmed;
    }

    public void Cancel() => Status = BookingStatus.Cancelled;
}
