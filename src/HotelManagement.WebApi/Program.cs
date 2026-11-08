using HotelManagement.Application.DTOs;
using HotelManagement.Application.Exceptions;
using HotelManagement.Application.UseCases;
using HotelManagement.Core.Entities;
using HotelManagement.Core.Interfaces;
using HotelManagement.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://localhost:5080");

builder.Services.AddSingleton<IRoomRepository, InMemoryRoomRepository>();
builder.Services.AddSingleton<IBookingRepository, InMemoryBookingRepository>();
builder.Services.AddSingleton<ICleanTaskRepository, InMemoryCleanTaskRepository>();
builder.Services.AddScoped<RoomService>();
builder.Services.AddScoped<BookingService>();
builder.Services.AddScoped<CleaningService>();

var app = builder.Build();

app.MapGet("/", () => Results.Ok(new
{
    application = "АИС «Гостиничный комплекс»",
    version = "1.0.0",
    endpoints = new[] { "GET /api/rooms", "GET /api/rooms/{id}", "POST /api/bookings", "POST /api/rooms/{id}/cleaning", "PATCH /api/rooms/{id}/status" }
}));

app.MapGet("/api/rooms", async (RoomService service, CancellationToken ct) =>
    Results.Ok(await service.GetRoomsAsync(ct)));

app.MapGet("/api/rooms/{id:int}", async (int id, RoomService service, CancellationToken ct) =>
{
    try { return Results.Ok(await service.GetRoomAsync(id, ct)); }
    catch (NotFoundException ex) { return Results.NotFound(new { error = ex.Message }); }
});

app.MapPost("/api/bookings", async (CreateBookingRequest request, BookingService service, CancellationToken ct) =>
{
    try
    {
        var id = await service.CreateAsync(request, ct);
        return Results.Created($"/api/bookings/{id}", new { bookingId = id, status = "Confirmed" });
    }
    catch (NotFoundException ex) { return Results.NotFound(new { error = ex.Message }); }
    catch (BusinessRuleException ex) { return Results.UnprocessableEntity(new { error = ex.Message }); }
});

app.MapPost("/api/rooms/{id:int}/cleaning", async (int id, CreateCleaningRequest request, CleaningService service, CancellationToken ct) =>
{
    try
    {
        var taskId = await service.RegisterCleaningAsync(id, request.EmployeeName, ct);
        return Results.Created($"/api/cleaning-tasks/{taskId}", new { taskId, roomId = id, status = "Cleaning" });
    }
    catch (NotFoundException ex) { return Results.NotFound(new { error = ex.Message }); }
    catch (BusinessRuleException ex) { return Results.UnprocessableEntity(new { error = ex.Message }); }
});

app.MapPatch("/api/rooms/{id:int}/status", async (int id, UpdateRoomStatusRequest request, RoomService service, CancellationToken ct) =>
{
    if (!Enum.TryParse<RoomStatus>(request.Status, true, out var status))
        return Results.BadRequest(new { error = "Недопустимый статус номера." });
    try { return Results.Ok(await service.ChangeStatusAsync(id, status, ct)); }
    catch (NotFoundException ex) { return Results.NotFound(new { error = ex.Message }); }
    catch (BusinessRuleException ex) { return Results.UnprocessableEntity(new { error = ex.Message }); }
});

app.Run();

public sealed record UpdateRoomStatusRequest(string Status);
public partial class Program { }
