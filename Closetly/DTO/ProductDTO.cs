using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Closetly.DTO
{
    public class ProductDTO
    {
        public Guid ProductId { get; set; }

        [Required(ErrorMessage = "A cor do produto é obrigatória.")]
        public string ProductColor { get; set; } = null!;
        [Required(ErrorMessage = "O tamanho do produto é obrigatório.")]
        public string ProductSize { get; set; } = null!;
        [Required(ErrorMessage = "O tipo do produto é obrigatório.")]
        public string ProductType { get; set; } = null!;
        [Required(ErrorMessage = "A ocasião é obrigatória.")]
        public string ProductOccasion { get; set; } = null!;
        public string ProductStatus { get; set; } = null!;
        [Range(0.01, double.MaxValue, ErrorMessage = "O valor do produto deve ser maior que zero.")]
        public decimal ProductValue { get; set; }
    }

    public class ProductFilters
    {
        public string? ProductColor { get; set; }

        public string? ProductSize { get; set; } 

        public string? ProductType { get; set; } 

        public string? ProductOccasion { get; set; } 
    }
}