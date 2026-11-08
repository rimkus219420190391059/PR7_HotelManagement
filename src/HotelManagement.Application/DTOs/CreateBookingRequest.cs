namespace HotelManagement.Application.DTOs;

public sealed record CreateBookingRequest(string GuestName, int RoomId, DateTime CheckIn, DateTime CheckOut);
public sealed record CreateCleaningRequest(string EmployeeName);
