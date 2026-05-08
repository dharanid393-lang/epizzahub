using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ePizza.Domain.Models
{
    public class OrderDomain
    {
        public string OrderId { get; set; }
        public string PaymentId { get; set; }
        public string Street { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
        public string Locality { get; set; }
        public string PhoneNumber { get; set; }
        public int UserId { get; set; }

        public ICollection<OrderItemDomain> OrderItems { get; set; } = [];
    }
}
