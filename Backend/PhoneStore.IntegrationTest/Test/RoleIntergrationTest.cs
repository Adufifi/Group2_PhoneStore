namespace PhoneStore.IntegrationTest
{
    public class RoleIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public RoleIntegrationTest(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task CreateRole_WithValidData_ShouldReturnNotFound()
        {
            // Arrange
            var roleDto = new RoleDto { RoleName = "User" };
            var content = new StringContent(
                JsonConvert.SerializeObject(roleDto),
                Encoding.UTF8,
                "application/json");

            // Act
            var response = await _client.PostAsync("/api/role/CreateRole", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task GetAllRoles_ShouldReturnSuccessAndValidData()
        {
            // Act
            var response = await _client.GetAsync("/api/role/All");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var jsonString = await response.Content.ReadAsStringAsync();
            var roles = JsonConvert.DeserializeObject<List<Role>>(jsonString);
            roles.Should().NotBeNull();
        }

        [Fact]
        public async Task GetRoleById_WithExistingId_ShouldReturnCorrectRole()
        {
            // Arrange
            var getAllResponse = await _client.GetAsync("/api/role/All");
            var jsonStringAllRole = await getAllResponse.Content.ReadAsStringAsync();
            var listRole = JsonConvert.DeserializeObject<List<Role>>(jsonStringAllRole);
            var firstRole = listRole!.FirstOrDefault();
            firstRole.Should().NotBeNull("Cần có ít nhất một role trong database để test");

            // Act
            var response = await _client.GetAsync($"/api/role/GetRoleById/{firstRole!.Id}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var jsonString = await response.Content.ReadAsStringAsync();
            var roleById = JsonConvert.DeserializeObject<Role>(jsonString);

            roleById.Should().NotBeNull();
            roleById!.Id.Should().Be(firstRole.Id);
            roleById.RoleName.Should().Be(firstRole.RoleName);
        }

        [Fact]
        public async Task DeleteRole_WithNonExistingId_ShouldReturnNotFound()
        {
            // Arrange
            var nonExistingId = Guid.NewGuid();

            // Act
            var response = await _client.DeleteAsync($"/api/role/DeleteById/{nonExistingId}");

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}