

namespace PhoneStore.Domain.ViewModel
{
    public class AuthResultVm
    {
        [JsonPropertyName("status")]
        public int status { get; set; }
        [JsonPropertyName("mess")]
        public string? mess { get; set; }
        [JsonPropertyName("token")]
        public string? Token { get; set; }
        [JsonPropertyName("refreshToken")]
        public string? RefreshToken { get; set; }
        [JsonPropertyName("expireAt")]
        public DateTime? ExpireAt { get; set; }
    }
}
