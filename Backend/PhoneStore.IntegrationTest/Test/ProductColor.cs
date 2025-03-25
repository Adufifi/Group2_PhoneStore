using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PhoneStore.Api.Controllers;
using PhoneStore.Application.Services.Interface;
using System.Net;

namespace PhoneStore.IntegrationTest.Test
{
    public class ProductColorControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ProductColorControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task GetAllColors_ReturnsSuccess()
        {
            var response = await _client.GetAsync("/api/productcolors/All");
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task CreateColor_ReturnsSuccess()
        {
            var newColor = new
            {
                ColorName = "TestColor",
                CreatedDate = DateTime.UtcNow
            };
            var content = new StringContent(JsonConvert.SerializeObject(newColor), Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("/api/productcolors/CreateColor", content);
            response.EnsureSuccessStatusCode();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task GetColorById_NotFound()
        {
            var id = Guid.NewGuid();
            var response = await _client.GetAsync($"/api/productcolors/GetColorById/{id}");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task DeleteColor_NotFound()
        {
            var id = Guid.NewGuid();
            var response = await _client.DeleteAsync($"/api/productcolors/DeleteById/{id}");

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task UpdateColor_NotFound()
        {
            var id = Guid.NewGuid();
            var updateColor = new
            {
                ColorName = "UpdatedColor",
                CreatedDate = DateTime.UtcNow
            };
            var content = new StringContent(JsonConvert.SerializeObject(updateColor), Encoding.UTF8, "application/json");

            var response = await _client.PutAsync($"/api/productcolors/UpdateById/{id}", content);

            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}
