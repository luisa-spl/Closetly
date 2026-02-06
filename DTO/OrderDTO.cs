using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Closetly.DTO
{
    public class OrderDTO
    {
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public DateTime? OrderedAt { get; set; }
        public DateTime ReturnDate { get; set; }
        public string OrderStatus { get; set; } = null!;
        public int? OrderTotalItems { get; set; }
    }
}