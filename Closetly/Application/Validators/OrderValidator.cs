using Closetly.DTO;
using Closetly.Models;
using Closetly.Repository.Interface;

namespace Closetly.Application.Validators;

public static class OrderValidator
{
    public static async Task<TbProduct?> VerifyProduct(IProductRepository _productRepository, OrderProductRequestDTO orderProduct) 
    {
        var product = await _productRepository.GetProductById(orderProduct.ProductId);
        
        if (product == null)
        {
            throw new InvalidOperationException($"Produto com Id: '{orderProduct.ProductId}' não encontrado");
        }

        return product;
    }

    public static void CheckProductStatusAndQuantity(TbProduct? product, int itemQuantity)
    {
        if (product.ProductStatus != ProductStatus.AVAILABLE)
        {
            throw new InvalidOperationException($"O produto '{product.ProductType}, com Id {product.ProductId}' não está disponível");
        }

        if (itemQuantity == 0)
        {
            throw new InvalidOperationException($"Você deve adicionar ao menos 1 unidade do produto: '{product.ProductType}, com Id {product.ProductId}' ");
        }
    }

    public static async Task<TbProduct?> ChangeProductStatus(IProductRepository _productRepository, TbProduct orderProduct)
    {
        var product = await _productRepository.GetProductById(orderProduct.ProductId);

        if (product == null)
        {
            throw new InvalidOperationException($"Produto com Id: '{orderProduct.ProductId}' não encontrado");
        }

        await _productRepository.UpdateProductStatus(product, ProductStatus.UNAVAILABLE);

        return product;
    }

    public static async Task ChangeManyProductsStatus(IProductRepository _productRepository, List<TbOrderProduct> orderProducts, string status)
    {
        foreach (var orderProduct in orderProducts)
        {
            var product = await _productRepository.GetProductById(orderProduct.ProductId);

            if (product == null)
            {
                throw new InvalidOperationException($"Produto com Id: '{orderProduct.ProductId}' não encontrado");
            }

            await _productRepository.UpdateProductStatus(product, status);
        }
    }
}
