using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Closetly.DTO;

namespace Closetly.Services.Interface
{
    public interface IProductService
    {
        public void CreateProduct(ProductDTO product);
        public Task<bool> UpdateProduct(Guid productId, UpdateProductDTO product);
        public List<ProductDTO> GetAvailableProducts(ProductFilters filters);
    }
}