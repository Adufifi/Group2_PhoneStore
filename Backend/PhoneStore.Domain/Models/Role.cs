namespace PhoneStore.Domain.Models
{
    [Table("Role")]
    public class Role : Base
    {
        [StringLength(100)]
        public required string RoleName { get; set; }
        public ICollection<Account> Accounts { get; set; } = new List<Account>();
    }
}