using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Closetly.DTO
{
    public class ProductDTO
    {
        public Guid ProductId { get; set; }

        public string ProductColor { get; set; } = null!;

        public string ProductSize { get; set; } = null!;

        public string ProductType { get; set; } = null!;

        public string ProductOccasion { get; set; } = null!;

        public string ProductStatus { get; set; } = null!;

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