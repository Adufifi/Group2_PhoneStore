using System.ComponentModel.DataAnnotations;

namespace PhoneStore.Application.Dto
{
    public class RoleDto
    {
        [StringLength(100)]
        public required string RoleName { get; set; }
    }
}
