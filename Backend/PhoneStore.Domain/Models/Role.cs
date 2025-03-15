namespace PhoneStore.Domain.Models
{
    [Table("Role")]
    public class Role : Base
    {
        public required string RoleName { get; set; }
        public ICollection<AccountRole> AccountRoles { get; set; } = new List<AccountRole>();
    }
}