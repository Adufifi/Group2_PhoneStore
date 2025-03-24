namespace PhoneStore.Domain.ModelRequest
{
    public class LoginVm
    {
        [Required, MaxLength(50)]
        public string Email { get; set; } = string.Empty;
        [Required, MaxLength(50)]
        public string Password { get; set; } = string.Empty;
    }
}
