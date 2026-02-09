using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Closetly.Repository.Interface;
using Closetly.Models;
using Microsoft.EntityFrameworkCore;
using Closetly.DTO;
using System.Runtime.CompilerServices;

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

        public async Task<TbProduct?> GetProductById(Guid id)
        {
            return await _context.TbProducts.FindAsync(id);
        }

        public void CreateProduct(ProductDTO product)
        {
            TbProduct newProduct = new TbProduct
            {
                ProductColor = product.ProductColor,
                ProductSize = product.ProductSize,
                ProductType = product.ProductType,
                ProductOccasion = product.ProductOccasion,
                ProductStatus = "disponível",
                ProductValue = product.ProductValue
            };

            _context.TbProducts.Add(newProduct);
            _context.SaveChanges();
        }

        public async Task UpdateProduct(TbProduct product)
        {
            _context.TbProducts.Update(product);
            await _context.SaveChangesAsync();
        }

    }
}