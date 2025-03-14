namespace PhoneStore.Domain.Models
{
    [Table("Account")]
    public class Account : Base
    {
        [StringLength(256)]
        public required string UserName { get; set; }
        public required string PassWord { get; set; }
        [StringLength(100)]
        public required string Email { get; set; }
        public byte[]? Img { get; set; }
        public string? NormalizedUserName { get; set; }
        public string? NormalizedEmail { get; set; }
        public bool? EmailConfirmed { get; set; }
        public Guid CartId { get; set; }
        public virtual Cart? Cart { get; set; }
        public ICollection<AccountRole> AccountRoles { get; set; } = new List<AccountRole>();
        public ICollection<Review> AccountReview { get; set; } = new List<Review>();
    }
}