namespace HotelManagement.Core.Entities;

public enum CleanTaskStatus
{
    Created,
    InProgress,
    Completed
}

public sealed class CleanTask
{
    public int Id { get; }
    public int RoomId { get; }
    public string EmployeeName { get; }
    public DateTime CreatedAt { get; }
    public CleanTaskStatus Status { get; private set; }

    public CleanTask(int id, int roomId, string employeeName)
    {
        if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));
        if (roomId <= 0) throw new ArgumentOutOfRangeException(nameof(roomId));
        if (string.IsNullOrWhiteSpace(employeeName)) throw new ArgumentException("Имя сотрудника обязательно.");
        Id = id;
        RoomId = roomId;
        EmployeeName = employeeName;
        CreatedAt = DateTime.UtcNow;
        Status = CleanTaskStatus.Created;
    }

    public void Complete() => Status = CleanTaskStatus.Completed;
}
