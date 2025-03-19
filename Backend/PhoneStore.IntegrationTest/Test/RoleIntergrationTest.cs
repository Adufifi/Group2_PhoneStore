

using Newtonsoft.Json;
using PhoneStore.Application.Dto;
using System.Text;

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
            var id = Guid.NewGuid();
            var testGetById = await client.GetAsync($"/api/role/GetRoleById/{id}");
            testGetById.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
            var roleDto = new RoleDto { RoleName = "User" };
            var content = new StringContent(JsonConvert.SerializeObject(roleDto), Encoding.UTF8, "application/json");
            var testAdd = await client.PostAsync($"/api/role/CreateRole", content);
            testAdd.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
    }
}