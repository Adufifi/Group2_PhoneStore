using System;
using System.ComponentModel.DataAnnotations;

namespace PhoneStore.Application.Dto
{
    public class ReviewDto
    {
        public Guid AccountId { get; set; }
        public Guid ProductId { get; set; }

        [StringLength(1000)]
        public string? Comment { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
