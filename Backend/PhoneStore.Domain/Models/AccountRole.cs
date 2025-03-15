namespace PhoneStore.Domain.Models
{
    [Table("AccountRole")]
    public class AccountRole
    {
        public required Guid AccountId { get; set; }
        public virtual required Account Account { get; set; }
        public required Guid RoleId { get; set; }
        public virtual required Role Role { get; set; }
    }
}