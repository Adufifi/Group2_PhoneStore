namespace PhoneStore.Domain.ModelRequest
{
    public class CheckRequest
    {
        [JsonPropertyName("userId")]
        public string? userId { get; set; }
    }
}
