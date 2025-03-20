namespace PhoneStore.Domain.Models
{
    [Table("RefreshToken")]
    public class RefreshToken
    {
        public int Id { get; set; }
        [ForeignKey("AccountId")]
        public Guid AccountId { get; set; }
        public string Token { get; set; } = string.Empty;
        public string? JwtId { get; set; }
        public bool IsRevoked { get; set; }
        public DateTime? DateAdded { get; set; }
        public DateTime? DateExpire { get; set; }
    }
}