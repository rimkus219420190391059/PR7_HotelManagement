using System.Net;
using Xunit;

namespace HotelManagement.IntegrationTests;

public sealed class RoomsApiTests : IClassFixture<WebApplicationFactoryFixture>
{
    private readonly HttpClient _client;

    public RoomsApiTests(WebApplicationFactoryFixture factory) => _client = factory.CreateClient();

    [Fact]
    public async Task GetRooms_ReturnsSeededRooms()
    {
        var response = await _client.GetAsync("/api/rooms");
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadAsStringAsync();
        Assert.Contains("\"101\"", body);
    }
}
