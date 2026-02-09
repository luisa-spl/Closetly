using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Closetly.DTO;

namespace Closetly.Repository.Interface
{
    public interface IProductRepository
    {
        public List<ProductDTO> GetAvailableProducts(ProductFilters filters);
        public void CreateProduct(ProductDTO product);
    }
}