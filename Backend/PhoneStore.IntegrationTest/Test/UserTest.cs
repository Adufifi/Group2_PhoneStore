
namespace PhoneStore.IntegrationTest.Test
{
    public class UserTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public UserTest(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }
        [Fact]
        public async Task CreateUser_WithDuplicateEmail_ShouldReturnConflict()
        {
            // Arrange
            var resignerAccount = new RegisterAccount()
            {
                Email = "hieunthe171211@gmail.com",
                Password = "12345678",
                UserName = "hieunthe171211"
            };
            var content = new StringContent(
                JsonConvert.SerializeObject(resignerAccount),
                Encoding.UTF8,
                "application/json");

            // Act
            var response = await _client.PostAsync("api/account/CreateAccount", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
    }
}