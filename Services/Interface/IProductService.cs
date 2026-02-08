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
        public List<ProductDTO> GetAvailableProducts();
    }
}