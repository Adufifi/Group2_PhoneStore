using System.Net;

namespace PhoneStore.IntegrationTest.Test
{
    public class ProductVariantsControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ProductVariantsControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAllProductVariants_ReturnsSuccess()
        {
            var response = await _client.GetAsync("/api/productvariants/All");
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task CreateProductVariant_ReturnsSuccess()
        {
            var newVariant = new
            {
                VariantName = "Test Variant",
                CreatedDate = DateTime.UtcNow
            };
            var content = new StringContent(JsonConvert.SerializeObject(newVariant), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/productvariants/CreateVariant", content);
            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetProductVariantById_NotFound()
        {
            var id = Guid.NewGuid();
            var response = await _client.GetAsync($"/api/productvariants/GetVariantById/{id}");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task DeleteProductVariant_NotFound()
        {
            var id = Guid.NewGuid();
            var response = await _client.DeleteAsync($"/api/productvariants/DeleteById/{id}");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task UpdateProductVariant_NotFound()
        {
            var id = Guid.NewGuid();
            var updateVariant = new
            {
                VariantName = "Updated Variant",
                CreatedDate = DateTime.UtcNow
            };
            var content = new StringContent(JsonConvert.SerializeObject(updateVariant), Encoding.UTF8, "application/json");

            var response = await _client.PutAsync($"/api/productvariants/UpdateById/{id}", content);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
