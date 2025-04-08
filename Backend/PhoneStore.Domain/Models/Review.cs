namespace PhoneStore.Domain.Models
{
    [Table("Review")]
    public class Review
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid AccountId { get; set; }
        public virtual Account? Account { get; set; }
        public Guid ProductId { get; set; }
        public virtual Product? Product { get; set; }
        [StringLength(1000)]
        public string? Comment { get; set; }
    }
}