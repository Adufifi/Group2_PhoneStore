namespace PhoneStore.IntegrationTest.Test
{
    public class Brand : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public Brand(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }
        [Fact]
        public void TestBrand()
        {
            var client = _factory.CreateClient();
            var idBrand = Guid.NewGuid();

        }
    }
}
