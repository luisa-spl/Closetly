using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Closetly.DTO
{
    public class PaymentDTO
    {
        public Guid OrderId { get; set; }

        public string PaymentType { get; set; } = null!;

        public decimal PaymentValue { get; set; }

        public string PaymentStatus { get; set; } = null!;    
    }
}