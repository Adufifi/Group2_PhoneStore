using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PhoneStore.Domain.Enums;

namespace PhoneStore.Domain.Models
{
    public class Order : Base
    {
        public Guid AccountId { get; set; }
        public virtual Account? Account { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public StatusProduct StatusProduct { get; set; }
        public string ShippingAddress { get; set; } = string.Empty;
        public string RecipientName { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}