using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Closetly.DTO
{
    public class PaymentDTO
    {
        public required Guid OrderId { get; set; }

        [Required]
        public required string PaymentType { get; set; }

        [Required]
        public required decimal PaymentValue { get; set; }
        
    }
}