

namespace PhoneStore.Domain.Models
{
    [Table("Capacity")]
    public class Capacity : Base
    {
        public required string CapacityName { get; set; }
    }
}