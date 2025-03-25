using System.Net;

namespace PhoneStore.IntegrationTest.Test
{
    public class CapacityControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public CapacityControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAllCapacities_ReturnsSuccess()
        {
            var response = await _client.GetAsync("/api/capacities/All");
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task CreateCapacity_ReturnsSuccess()
        {
            var newCapacity = new
            {
                CapacityName = "Test Capacity",
                CreatedDate = DateTime.UtcNow
            };
            var content = new StringContent(JsonConvert.SerializeObject(newCapacity), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/capacities/CreateCapacity", content);
            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetCapacityById_NotFound()
        {
            var id = Guid.NewGuid();
            var response = await _client.GetAsync($"/api/capacities/GetCapacityById/{id}");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task DeleteCapacity_NotFound()
        {
            var id = Guid.NewGuid();
            var response = await _client.DeleteAsync($"/api/capacities/DeleteById/{id}");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task UpdateCapacity_NotFound()
        {
            var id = Guid.NewGuid();
            var updateCapacity = new
            {
                CapacityName = "Updated Capacity",
                CreatedDate = DateTime.UtcNow
            };
            var content = new StringContent(JsonConvert.SerializeObject(updateCapacity), Encoding.UTF8, "application/json");

            var response = await _client.PutAsync($"/api/capacities/UpdateById/{id}", content);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
