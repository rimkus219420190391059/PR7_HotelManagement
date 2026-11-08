using HotelManagement.Application.DTOs;
using HotelManagement.Application.Exceptions;
using HotelManagement.Application.UseCases;
using HotelManagement.Core.Entities;
using HotelManagement.Core.Interfaces;
using Moq;
using Xunit;

namespace HotelManagement.UnitTests;

public sealed class BookingServiceTests
{
    [Fact]
    public async Task CreateAsync_ValidRequest_CreatesBooking()
    {
        var room = new Room(101, "101", 2, 4500m);
        var rooms = new Mock<IRoomRepository>();
        var bookings = new Mock<IBookingRepository>();
        rooms.Setup(x => x.GetByIdAsync(101, It.IsAny<CancellationToken>())).ReturnsAsync(room);
        bookings.Setup(x => x.HasOverlapAsync(101, It.IsAny<DateTime>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>())).ReturnsAsync(false);

        var service = new BookingService(bookings.Object, rooms.Object);
        var id = await service.CreateAsync(new CreateBookingRequest("Иван Петров", 101,
            new DateTime(2026, 11, 10), new DateTime(2026, 11, 12)));

        Assert.True(id > 0);
        bookings.Verify(x => x.AddAsync(It.IsAny<Booking>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_UnknownRoom_ThrowsNotFound()
    {
        var rooms = new Mock<IRoomRepository>();
        var bookings = new Mock<IBookingRepository>();
        rooms.Setup(x => x.GetByIdAsync(999, It.IsAny<CancellationToken>())).ReturnsAsync((Room?)null);

        var service = new BookingService(bookings.Object, rooms.Object);
        await Assert.ThrowsAsync<NotFoundException>(() => service.CreateAsync(
            new CreateBookingRequest("Иван Петров", 999, DateTime.Today, DateTime.Today.AddDays(1))));
    }

    [Fact]
    public async Task CreateAsync_InvalidDateRange_ThrowsBusinessRule()
    {
        var room = new Room(101, "101", 2, 4500m);
        var rooms = new Mock<IRoomRepository>();
        var bookings = new Mock<IBookingRepository>();
        rooms.Setup(x => x.GetByIdAsync(101, It.IsAny<CancellationToken>())).ReturnsAsync(room);

        var service = new BookingService(bookings.Object, rooms.Object);
        await Assert.ThrowsAsync<BusinessRuleException>(() => service.CreateAsync(
            new CreateBookingRequest("Иван Петров", 101, DateTime.Today.AddDays(2), DateTime.Today)));
    }
}
