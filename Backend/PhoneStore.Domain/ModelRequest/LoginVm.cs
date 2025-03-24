namespace PhoneStore.Domain.ModelRequest
{
    public class LoginVm
    {
        [Required, MaxLength(50)]
        [JsonPropertyName("email")]
        public string Email { get; set; } = string.Empty;
        [Required, MaxLength(50)]
        [JsonPropertyName("password")]
        public string Password { get; set; } = string.Empty;
    }
}
