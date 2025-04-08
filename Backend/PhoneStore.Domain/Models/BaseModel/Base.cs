namespace PhoneStore.Domain.Models
{
    public abstract class Base
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}