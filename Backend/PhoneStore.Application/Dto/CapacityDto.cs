using System.ComponentModel.DataAnnotations;

namespace PhoneStore.Application.Dto
{
    public class CapacityDto
    {
        public Guid Id { get; set; }

        [StringLength(100)]
        public required string CapacityName { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
