namespace PhoneStore.Domain.Models
{
    [Table("RefreshToken")]
    public class RefreshToken
    {
        public int Id { get; set; }
        public Guid AccountId { get; set; }
        public string Token { get; set; } = string.Empty;
        public bool IsRevoked { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? DateExpire { get; set; }

        [ForeignKey(nameof(AccountId))]
        public virtual Account Account { get; set; } = null!;
    }
}