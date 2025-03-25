using System.ComponentModel.DataAnnotations;

namespace PhoneStore.Application.Dto
{
    public class ColorDto
    {
        public Guid Id { get; set; }

        [StringLength(100)]
        public required string ColorName { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
