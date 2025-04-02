namespace PhoneStore.IntegrationTest.Test
{
    public class BrandTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly string _baseUrl = "api/brand";

        public BrandTest(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetAllBrands_ShouldReturnSuccess()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync($"{_baseUrl}/AllBrand");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetBrandById_WithValidId_ShouldReturnBrand()
        {
            // Arrange
            var client = _factory.CreateClient();
            var brandId = await CreateTestBrand(client);

            // Act
            var response = await client.GetAsync($"{_baseUrl}/GetBrandById/{brandId}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetBrandById_WithInvalidId_ShouldReturnNotFound()
        {
            // Arrange
            var client = _factory.CreateClient();
            var invalidId = Guid.NewGuid();

            // Act
            var response = await client.GetAsync($"{_baseUrl}/GetBrandById/{invalidId}");

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task CreateBrand_WithValidData_ShouldReturnSuccess()
        {
            // Arrange
            var client = _factory.CreateClient();
            var brandDto = new BrandDto
            {
                Name = "Test Brand"
            };

            // Act
            var response = await client.PostAsJsonAsync($"{_baseUrl}/CreateBrand", brandDto);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<StatusResponse>();
            Assert.NotNull(result);
            Assert.Equal(1, result.status);
            Assert.Equal("Add success", result.mess);
        }

        [Fact]
        public async Task UpdateBrand_WithValidData_ShouldReturnSuccess()
        {
            // Arrange
            var client = _factory.CreateClient();
            var brandId = await CreateTestBrand(client);
            var updateDto = new BrandDto
            {
                Name = "Updated Brand"
            };

            // Act
            var response = await client.PutAsJsonAsync($"{_baseUrl}/UpdateById/{brandId}", updateDto);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<StatusResponse>();
            Assert.NotNull(result);
            Assert.Equal(1, result.status);
            Assert.Equal("Update success", result.mess);
        }

        [Fact]
        public async Task DeleteBrand_WithValidId_ShouldReturnSuccess()
        {
            // Arrange
            var client = _factory.CreateClient();
            var brandId = await CreateTestBrand(client);

            // Act
            var response = await client.DeleteAsync($"{_baseUrl}/DeleteById/{brandId}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<StatusResponse>();
            Assert.NotNull(result);
            Assert.Equal(1, result.status);
            Assert.Equal("Delete success", result.mess);
        }

        private async Task<Guid> CreateTestBrand(HttpClient client)
        {
            var brandDto = new BrandDto
            {
                Name = "Test Brand"
            };

            var response = await client.PostAsJsonAsync($"{_baseUrl}/CreateBrand", brandDto);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            // Get all brands and find the one we just created
            var brandsResponse = await client.GetAsync($"{_baseUrl}/AllBrand");
            Assert.Equal(HttpStatusCode.OK, brandsResponse.StatusCode);
            var brands = await brandsResponse.Content.ReadFromJsonAsync<List<Domain.Models.Brand>>();
            Assert.NotNull(brands);
            var testBrand = brands.FirstOrDefault(b => b.Name == "Test Brand");
            Assert.NotNull(testBrand);
            return testBrand.Id;
        }
    }
}
