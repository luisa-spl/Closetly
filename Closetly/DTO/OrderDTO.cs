using Closetly.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

    public class OrderRequestDTO
    {
        [Required]
        public Guid UserId { get; set; }
        [Required]
        [AllowedValues(3, 7, 14, ErrorMessage = "O período de locação deve ser de 3, 7 ou 14 dias.")]
        public int ReturnPeriod { get; set; }
        [Required]
        [MinLength(1, ErrorMessage = "O pedido deve conter pelo menos um produto.")]
        public List<OrderProductRequestDTO> Products { get; set; } = new();
    }
    public class OrderResponseDTO
    {
        public Guid Id { get; set; }
        public DateTime? OrderedAt { get; set; }
        public DateTime ReturnDate { get; set; }    
        public string PaymentStatus { get; set; }
        public Guid PaymentId { get; set; }
        public decimal Total { get; set; }
        public Guid UserId { get; set; }
        public List<OrderProductResponseDTO> Products { get; set; } = new();
    }

    public class OrderProductRequestDTO
    {
        public Guid ProductId { get; set; }
        [AllowedValues(1, ErrorMessage = "Você pode alugar apenas 1 unidade de cada item.")]
        public int Quantity { get; set; }
    }

    public class OrderProductResponseDTO
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
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

    public class OrderWithProductsRequestDTO
    {
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public Guid OrderId { get; set; }
    }

}