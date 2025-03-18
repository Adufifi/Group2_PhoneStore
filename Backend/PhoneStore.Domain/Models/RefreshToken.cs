namespace PhoneStore.Domain.Models
{
    public class RefreshToken
    {
        public int Id { get; set; }
        public Guid AccountId { get; set; }
        public virtual Account? Account { get; set; }
        public string Token { get; set; } = string.Empty;
        public string JwtId { get; set; } = string.Empty;
        public bool IsRevoked { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime DateExpire { get; set; }
    }
}