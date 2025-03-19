using System.Text.Json.Serialization;

namespace PhoneStore.Domain.ViewModel
{
    public class StatusResponse
    {
        [JsonPropertyName("status")]
        public int status { get; set; }
        [JsonPropertyName("mess")]
        public string mess { get; set; } = string.Empty;
    }
}
