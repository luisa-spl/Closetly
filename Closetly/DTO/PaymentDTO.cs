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
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor do pagamento deve ser maior que zero.")]
        public required decimal PaymentValue { get; set; }
        
    }

    public class CreatePaymentDTO
    {
        public required Guid OrderId { get; set; }

        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor do produto deve ser maior que zero.")]
        public required decimal PaymentValue { get; set; }

    }
}