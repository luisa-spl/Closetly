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

    public sealed class OrderProduct
    {
        public Guid ProductId { get; set; }
        public string ProductType { get; set; } = "";
        public int Quantity { get; set; }
    }

    public sealed class OrderPayment
    {
        public Guid PaymentId { get; set; }
        public string PaymentType { get; set; } = "";
        public decimal PaymentValue { get; set; }
        public string PaymentStatus { get; set; } = "";
    }

}