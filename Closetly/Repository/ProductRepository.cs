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

    public List<ProductDTO> GetAvailableProducts(ProductFilters? filters)
    {
        var query = _context.TbProducts
            .AsNoTracking()
            .Where(p => p.ProductStatus == ProductStatus.AVAILABLE);

        // se filters vier null, vira um objeto vazio
        filters ??= new ProductFilters();

        if (!string.IsNullOrWhiteSpace(filters.ProductType))
            query = query.Where(p => p.ProductType == filters.ProductType);

        if (!string.IsNullOrWhiteSpace(filters.ProductOccasion))
            query = query.Where(p => p.ProductOccasion == filters.ProductOccasion);

        if (!string.IsNullOrWhiteSpace(filters.ProductSize))
            query = query.Where(p => p.ProductSize == filters.ProductSize);

        if (!string.IsNullOrWhiteSpace(filters.ProductColor))
            query = query.Where(p => p.ProductColor == filters.ProductColor);

        return query.Select(p => new ProductDTO
        {
            ProductId = p.ProductId,
            ProductColor = p.ProductColor,
            ProductSize = p.ProductSize,
            ProductType = p.ProductType,
            ProductOccasion = p.ProductOccasion,
            ProductStatus = p.ProductStatus,
            ProductValue = p.ProductValue
        }).ToList();
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
                ProductStatus = ProductStatus.AVAILABLE,
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

        public async Task DeleteProduct(TbProduct product) 
        {
            _context.TbProducts.Update(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateProductStatus(TbProduct product, string status)
        {
            product.ProductStatus = status;
            _context.TbProducts.Update(product);
            await _context.SaveChangesAsync();
        }

    }
}