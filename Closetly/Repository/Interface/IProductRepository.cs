using Closetly.DTO;
using Closetly.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Closetly.Repository.Interface
{
    public interface IProductRepository
    {
        public List<ProductDTO> GetAvailableProducts(ProductFilters filters);
        public Task<TbProduct?> GetProductById(Guid id);
        public void CreateProduct(ProductDTO product);
        public Task UpdateProduct(TbProduct product);
        public Task DeleteProduct(TbProduct product);
        public Task UpdateProductStatus(TbProduct product, string status);

    }
}