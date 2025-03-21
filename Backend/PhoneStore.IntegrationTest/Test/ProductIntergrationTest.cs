using Newtonsoft.Json;
using PhoneStore.Application.Dto;
using System.Text;

namespace PhoneStore.IntegrationTest.Test
{
    public class ProductIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public ProductIntegrationTest(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetAllProducts_Test()
        {
            var client = _factory.CreateClient();

            // Gửi yêu cầu GET để lấy tất cả sản phẩm
            var response = await client.GetAsync("/api/product/All");

            // Kiểm tra xem mã trạng thái có phải là OK không
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            // Kiểm tra nội dung trả về
            var responseContent = await response.Content.ReadAsStringAsync();
            responseContent.Should().Contain("Product");
        }

        [Fact]
        public async Task GetProductById_Test()
        {
            var client = _factory.CreateClient();
            var productId = Guid.NewGuid();  // ID của sản phẩm giả

            // Gửi yêu cầu GET để lấy sản phẩm theo ID
            var response = await client.GetAsync($"/api/product/GetProductById/{productId}");

            // Kiểm tra xem mã trạng thái có phải là NotFound (404) nếu không tìm thấy
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);

            // Tạo một ProductDto giả để thêm vào hệ thống
            var productDto = new ProductDto
            {
                BrandId = Guid.NewGuid(),
                ProductName = "Test Product",
                Description = "This is a test product",
                Stock = 100,
                IsPromoted = false,
                BuyCount = 50
            };

            var content = new StringContent(JsonConvert.SerializeObject(productDto), Encoding.UTF8, "application/json");

            // Thêm sản phẩm mới
            var postResponse = await client.PostAsync("/api/product/CreateProduct", content);
            postResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            // Gửi lại yêu cầu GET để lấy sản phẩm đã thêm
            var getResponse = await client.GetAsync($"/api/product/GetProductById/{productId}");
            getResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task CreateProduct_Test()
        {
            var client = _factory.CreateClient();

            // Tạo một ProductDto giả
            var productDto = new ProductDto
            {
                BrandId = Guid.NewGuid(),
                ProductName = "New Product",
                Description = "This is a new product",
                Stock = 200,
                IsPromoted = true,
                BuyCount = 100
            };

            var content = new StringContent(JsonConvert.SerializeObject(productDto), Encoding.UTF8, "application/json");

            // Gửi yêu cầu POST để tạo sản phẩm mới
            var response = await client.PostAsync("/api/product/CreateProduct", content);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            // Kiểm tra nội dung trả về
            var responseContent = await response.Content.ReadAsStringAsync();
            responseContent.Should().Contain("Add success");
        }

        [Fact]
        public async Task UpdateProduct_Test()
        {
            var client = _factory.CreateClient();
            var productId = Guid.NewGuid();  // ID của sản phẩm giả

            // Tạo một ProductDto giả
            var productDto = new ProductDto
            {
                BrandId = Guid.NewGuid(),
                ProductName = "Updated Product",
                Description = "Updated product description",
                Stock = 150,
                IsPromoted = true,
                BuyCount = 120
            };

            var content = new StringContent(JsonConvert.SerializeObject(productDto), Encoding.UTF8, "application/json");

            // Cập nhật sản phẩm bằng PUT
            var response = await client.PutAsync($"/api/product/UpdateById/{productId}", content);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound); // Kiểm tra lỗi khi không tìm thấy sản phẩm

            // Tạo một sản phẩm hợp lệ và thử cập nhật lại
            var validProductDto = new ProductDto
            {
                BrandId = Guid.NewGuid(),
                ProductName = "Valid Product",
                Description = "This is a valid product",
                Stock = 200,
                IsPromoted = true,
                BuyCount = 150
            };

            var validContent = new StringContent(JsonConvert.SerializeObject(validProductDto), Encoding.UTF8, "application/json");
            var validResponse = await client.PostAsync("/api/product/CreateProduct", validContent);
            validResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var updateResponse = await client.PutAsync($"/api/product/UpdateById/{productId}", validContent);
            updateResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task DeleteProduct_Test()
        {
            var client = _factory.CreateClient();
            var productId = Guid.NewGuid();  // ID của sản phẩm giả

            // Gửi yêu cầu DELETE để xóa sản phẩm
            var response = await client.DeleteAsync($"/api/product/DeleteById/{productId}");

            // Kiểm tra xem mã trạng thái có phải là NotFound (404) nếu không tìm thấy sản phẩm không
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);

            // Tạo một sản phẩm hợp lệ và thử xóa
            var productDto = new ProductDto
            {
                BrandId = Guid.NewGuid(),
                ProductName = "Product to Delete",
                Description = "This product will be deleted",
                Stock = 50,
                IsPromoted = false,
                BuyCount = 20
            };

            var content = new StringContent(JsonConvert.SerializeObject(productDto), Encoding.UTF8, "application/json");
            var postResponse = await client.PostAsync("/api/product/CreateProduct", content);
            postResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var deleteResponse = await client.DeleteAsync($"/api/product/DeleteById/{productId}");
            deleteResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
    }
}
