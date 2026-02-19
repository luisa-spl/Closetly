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

        public async Task UpdateProduct(Guid productId, UpdateProductDTO product)
        {
            var existingProduct = await _repository.GetProductById(productId);

            if (existingProduct == null)
            {
                throw new InvalidOperationException("Produto não encontrado.");
            }

            if (product.ProductValue.HasValue) existingProduct.ProductValue = product.ProductValue.Value;
            if (product.ProductStatus != null) existingProduct.ProductStatus = product.ProductStatus;
            if (product.ProductType != null) existingProduct.ProductType = product.ProductType;
            if (product.ProductColor != null) existingProduct.ProductColor = product.ProductColor;
            if (product.ProductOccasion != null) existingProduct.ProductOccasion = product.ProductOccasion;
            if (product.ProductSize != null) existingProduct.ProductSize = product.ProductSize;

            await _repository.UpdateProduct(existingProduct);
        }

        public async Task DeleteProduct(Guid productId)
        {
            var existingProduct = await _repository.GetProductById(productId);

            if (existingProduct == null) 
            {
                throw new InvalidOperationException("Produto não encontrado.");
            }

            if (existingProduct.ProductStatus == ProductStatus.DELETED)
            {
                throw new InvalidOperationException("O produto já foi deletado.");
            }

            existingProduct.ProductStatus = ProductStatus.DELETED;

            await _repository.DeleteProduct(existingProduct);
        }
    }
}