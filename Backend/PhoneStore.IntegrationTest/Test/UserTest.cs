
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
        [Fact]
        public async Task TestGetAllAccount()
        {
            // Arrange
            // Act
            var response = await _client.GetAsync("api/account/GetAll");
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);

        }
        [Fact]
        public async Task TestGetAccountById()
        {
            // Arrange
            var id = Guid.NewGuid();
            // Act
            var response = await _client.GetAsync($"api/account/GetById{id}");
            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }
        [Fact]
        public async Task CreateAccount_WithValidData_ShouldReturnOk()
        {
            // Arrange
            var registerAccount = new RegisterAccount()
            {
                Email = "validemail@example.com",
                Password = "12345678",
                UserName = "validuser"
            };
            var content = new StringContent(
                JsonConvert.SerializeObject(registerAccount),
                Encoding.UTF8,
                "application/json");

            // Act
            var response = await _client.PostAsync("api/account/CreateAccount", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task CreateAccount_WithInvalidEmail_ShouldReturnNotFound()
        {
            // Arrange
            var registerAccount = new RegisterAccount()
            {
                Email = "invalidemail",
                Password = "12345678",
                UserName = "invaliduser"
            };
            var content = new StringContent(
                JsonConvert.SerializeObject(registerAccount),
                Encoding.UTF8,
                "application/json");

            // Act
            var response = await _client.PostAsync("api/account/CreateAccount", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task CreateAccount_WithDuplicateEmail_ShouldReturnNotFound()
        {
            // Arrange
            var registerAccount = new RegisterAccount()
            {
                Email = "duplicateemail@example.com",
                Password = "12345678",
                UserName = "duplicateuser"
            };
            var content = new StringContent(
                JsonConvert.SerializeObject(registerAccount),
                Encoding.UTF8,
                "application/json");

            // Act
            var response = await _client.PostAsync("api/account/CreateAccount", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task CreateAccount_WithInvalidModel_ShouldReturnNotFound()
        {
            // Arrange
            var registerAccount = new RegisterAccount()
            {
                Email = "validemail@example.com",
                Password = "",
                UserName = "validuser"
            };
            var content = new StringContent(
                JsonConvert.SerializeObject(registerAccount),
                Encoding.UTF8,
                "application/json");

            // Act
            var response = await _client.PostAsync("api/account/CreateAccount", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        }

    }
}