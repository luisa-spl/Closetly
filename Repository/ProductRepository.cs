using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Closetly.Repository.Interface;
using Closetly.Models;
using Microsoft.EntityFrameworkCore;
using Closetly.DTO;

namespace Closetly.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly PostgresContext _context;

        public ProductRepository(PostgresContext context)
        {
            _context = context;
        }

        public List<ProductDTO> GetAvailableProducts()
        {
            var availableProducts = _context.TbProducts
                    .AsNoTracking()
                    .Where(p => p.ProductStatus == "disponível")
                    .Select(p => new ProductDTO
                    {
                        ProductId = p.ProductId,
                        ProductColor = p.ProductColor,
                        ProductSize = p.ProductSize,
                        ProductType = p.ProductType,
                        ProductOccasion = p.ProductOccasion,
                        ProductStatus = p.ProductStatus,
                        ProductValue = p.ProductValue
                    }).ToList();
                    
            return availableProducts;
        }
    }
}