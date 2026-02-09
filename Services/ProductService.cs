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

        public List<ProductDTO> GetAvailableProducts()
        {
            return _repository.GetAvailableProducts();
        }

        public void CreateProduct(ProductDTO product)
        {
            if(product == null)
            {
                throw new ArgumentException("Produto não pode ser nulo");
            }
            _repository.CreateProduct(product);
        }

        public async Task<bool> UpdateProduct(Guid productId, UpdateProductDTO product)
        {
            var existingProduct = await _repository.GetProductById(productId);

            if (existingProduct == null) return false;

            if (product.ProductValue.HasValue) existingProduct.ProductValue = product.ProductValue.Value;
            if (product.ProductStatus != null) existingProduct.ProductStatus = product.ProductStatus;
            if (product.ProductType != null) existingProduct.ProductType = product.ProductType;
            if (product.ProductColor != null) existingProduct.ProductColor = product.ProductColor;
            if (product.ProductOccasion != null) existingProduct.ProductOccasion = product.ProductOccasion;
            if (product.ProductSize != null) existingProduct.ProductSize = product.ProductSize;

            _repository.UpdateProduct(existingProduct);
            return true;
        }

    }


}