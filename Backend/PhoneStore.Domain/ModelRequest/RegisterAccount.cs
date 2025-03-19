namespace PhoneStore.Domain.ViewModel
{
    public class RegisterAccount
    {
        [StringLength(100)]
        public string Email { get; set; } = default!;
        [StringLength(50)]
        public string UserName { get; set; } = default!;
        [StringLength(50)]
        public string Password { get; set; } = default!;
    }
}
