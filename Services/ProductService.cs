using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Closetly.Services.Interface;
using Closetly.DTO;
using Closetly.Models;
using Closetly.Repository.Interface;

namespace Closetly.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository productRepository)
        {
            _repository = productRepository;
        }

        public List<ProductDTO> GetAvailableProducts(ProductFilters filters)
        {
            return _repository.GetAvailableProducts(filters);
        }

        public void CreateProduct(ProductDTO product)
        {
            if(product == null)
            {
                throw new ArgumentException("Produto não pode ser nulo");
            }
            _repository.CreateProduct(product);
        }
    }


}