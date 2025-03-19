using System.Net;

namespace PhoneStore.IntegrationTest
{
    public class RoleIntergrationTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public RoleIntergrationTest(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }
        [Fact]
        public async Task RoleTest()
        {
            var client = _factory.CreateClient();
            var roleDto = new RoleDto { RoleName = "User" };
            var content = new StringContent(JsonConvert.SerializeObject(roleDto), Encoding.UTF8, "application/json");
            var testAdd = await client.PostAsync($"/api/role/CreateRole", content);
            testAdd.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
            var testGetAllRole = await client.GetAsync("/api/role/All");
            testGetAllRole.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
            var jsonStringAllRole = await testGetAllRole.Content.ReadAsStringAsync();
            var listRole = JsonConvert.DeserializeObject<List<Role>>(jsonStringAllRole);
            var firstRole = listRole!.FirstOrDefault();
            var testGetById = await client.GetAsync($"/api/role/GetRoleById/{firstRole.Id}");
            testGetById.StatusCode.Should().Be(HttpStatusCode.OK);

        }
        [Fact]
        public async Task RoleDelete()
        {
            var client = _factory.CreateClient();
            var id = Guid.NewGuid();
            var testDeleteById = await client.DeleteAsync($"/api/role/DeleteById/{id}");
            testDeleteById.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}