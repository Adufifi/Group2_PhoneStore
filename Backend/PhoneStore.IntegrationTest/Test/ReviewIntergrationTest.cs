using Newtonsoft.Json;
using PhoneStore.Application.Dto;
using System.Text;

namespace PhoneStore.IntegrationTest.Test
{
    public class ReviewIntergrationTest : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public ReviewIntergrationTest(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetAllReviews_Test()
        {
            var client = _factory.CreateClient();

            // Gửi yêu cầu GET để lấy tất cả các đánh giá
            var response = await client.GetAsync("/api/review/All");

            // Kiểm tra xem mã trạng thái có phải là OK không
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            // Kiểm tra nội dung trả về (giả sử có ít nhất một đánh giá)
            var responseContent = await response.Content.ReadAsStringAsync();
            responseContent.Should().Contain("reviews");
        }

        [Fact]
        public async Task GetReviewById_Test()
        {
            var client = _factory.CreateClient();
            var reviewId = Guid.NewGuid();  // ID của đánh giá giả

            // Gửi yêu cầu GET để lấy đánh giá theo ID
            var response = await client.GetAsync($"/api/review/GetReviewById/{reviewId}");

            // Kiểm tra mã trạng thái trả về là NotFound (404) nếu không tìm thấy
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);

            // Tạo một review DTO giả để thêm vào hệ thống
            var reviewDto = new ReviewDto
            {
                ProductId = Guid.NewGuid(), // ID của sản phẩm
                AccountId = Guid.NewGuid(), // ID của tài khoản
                Comment = "Great product!",  // Bình luận
                CreatedDate = DateTime.Now   // Ngày tạo
            };

            var content = new StringContent(JsonConvert.SerializeObject(reviewDto), Encoding.UTF8, "application/json");

            // Thêm một đánh giá mới
            var postResponse = await client.PostAsync("/api/review/CreateReview", content);
            postResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            // Gửi lại yêu cầu GET để lấy review đã thêm
            var getResponse = await client.GetAsync($"/api/review/GetReviewById/{reviewId}");
            getResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task CreateReview_Test()
        {
            var client = _factory.CreateClient();

            // Tạo một ReviewDto giả
            var reviewDto = new ReviewDto
            {
                ProductId = Guid.NewGuid(), // ID của sản phẩm
                AccountId = Guid.NewGuid(), // ID của tài khoản
                Comment = "Excellent product!", // Bình luận
                CreatedDate = DateTime.Now  // Ngày tạo
            };

            var content = new StringContent(JsonConvert.SerializeObject(reviewDto), Encoding.UTF8, "application/json");

            // Gửi yêu cầu POST để tạo một đánh giá mới
            var response = await client.PostAsync("/api/review/CreateReview", content);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            // Kiểm tra nội dung trả về
            var responseContent = await response.Content.ReadAsStringAsync();
            responseContent.Should().Contain("Add success");
        }

        [Fact]
        public async Task UpdateReview_Test()
        {
            var client = _factory.CreateClient();
            var reviewId = Guid.NewGuid();  // ID của review giả

            // Tạo một ReviewDto giả
            var reviewDto = new ReviewDto
            {
                ProductId = Guid.NewGuid(), // ID của sản phẩm
                AccountId = Guid.NewGuid(), // ID của tài khoản
                Comment = "Good product, but could be better.", // Bình luận
                CreatedDate = DateTime.Now  // Ngày tạo
            };

            var content = new StringContent(JsonConvert.SerializeObject(reviewDto), Encoding.UTF8, "application/json");

            // Cập nhật đánh giá bằng PUT
            var response = await client.PutAsync($"/api/review/UpdateReview/{reviewId}", content);
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound); // Kiểm tra lỗi khi không tìm thấy review

            // Tạo một review thật và cập nhật lại
            var validReviewDto = new ReviewDto
            {
                ProductId = Guid.NewGuid(),
                AccountId = Guid.NewGuid(),
                Comment = "Amazing product!",
                CreatedDate = DateTime.Now
            };

            var validContent = new StringContent(JsonConvert.SerializeObject(validReviewDto), Encoding.UTF8, "application/json");
            var validResponse = await client.PostAsync("/api/review/CreateReview", validContent);
            validResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var validReviewId = Guid.NewGuid(); // Giả sử reviewId đã được thêm thành công
            var updateResponse = await client.PutAsync($"/api/review/UpdateReview/{validReviewId}", validContent);
            updateResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task DeleteReview_Test()
        {
            var client = _factory.CreateClient();
            var reviewId = Guid.NewGuid();  // ID của review giả
            var accountId = Guid.NewGuid();  // ID của tài khoản giả
            var productId = Guid.NewGuid();  // ID của sản phẩm giả

            // Gửi yêu cầu DELETE để xóa một đánh giá
            var response = await client.DeleteAsync($"/api/review/DeleteReview/{reviewId}?accountId={accountId}&productId={productId}");

            // Kiểm tra xem mã trạng thái có phải là NotFound (404) nếu không tìm thấy review không
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);

            // Tạo một review thật và thử xóa
            var reviewDto = new ReviewDto
            {
                ProductId = Guid.NewGuid(),
                AccountId = Guid.NewGuid(),
                Comment = "Awesome product!",
                CreatedDate = DateTime.Now
            };

            var content = new StringContent(JsonConvert.SerializeObject(reviewDto), Encoding.UTF8, "application/json");
            var postResponse = await client.PostAsync("/api/review/CreateReview", content);
            postResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);

            var deleteResponse = await client.DeleteAsync($"/api/review/DeleteReview/{reviewId}?accountId={accountId}&productId={productId}");
            deleteResponse.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }
    }
}
