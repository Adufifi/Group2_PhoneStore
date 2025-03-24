using System.Text.Json.Serialization;

namespace PhoneStore.Domain.ModelResponse
{
    public class AccountResponse
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        [JsonPropertyName("userName")]
        [StringLength(256)]
        public required string UserName { get; set; }
        [JsonPropertyName("email")]
        [StringLength(100)]
        public required string Email { get; set; }
        public byte[]? Img { get; set; }

    }
}
