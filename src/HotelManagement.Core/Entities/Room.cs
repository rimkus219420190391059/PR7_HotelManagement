namespace HotelManagement.Core.Entities;

public enum RoomStatus
{
    Dirty,
    Cleaning,
    Clean,
    Occupied
}

public sealed class Room
{
    public int Id { get; private set; }
    public string Number { get; private set; }
    public int Capacity { get; private set; }
    public decimal PricePerNight { get; private set; }
    public RoomStatus Status { get; private set; }

    public Room(int id, string number, int capacity, decimal pricePerNight, RoomStatus status = RoomStatus.Clean)
    {
        if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));
        if (string.IsNullOrWhiteSpace(number)) throw new ArgumentException("Номер комнаты обязателен.", nameof(number));
        if (capacity <= 0) throw new ArgumentOutOfRangeException(nameof(capacity));
        if (pricePerNight <= 0) throw new ArgumentOutOfRangeException(nameof(pricePerNight));

        Id = id;
        Number = number;
        Capacity = capacity;
        PricePerNight = pricePerNight;
        Status = status;
    }

    public void SetStatus(RoomStatus status) => Status = status;
}
