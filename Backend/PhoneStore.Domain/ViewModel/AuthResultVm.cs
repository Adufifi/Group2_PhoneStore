using System.Text.Json.Serialization;

namespace PhoneStore.Domain.ViewModel
{
    public class AuthResultVm
    {
        [JsonPropertyName("Token")]
        public string Token { get; set; } = string.Empty;
        [JsonPropertyName("RefreshToken")]
        public string RefreshToken { get; set; } = string.Empty;
        [JsonPropertyName("ExpireAt")]
        public DateTime ExpireAt { get; set; }
    }
}
