using Closetly.DTO;
using Closetly.Models;
using Closetly.Repository.Interface;
using Closetly.Services;
using Moq;

namespace Closetly.Tests.Services;

[TestFixture]
internal class ProductServiceTest
{
    private Mock<IProductRepository> _productRepositoryMock;
    private ProductService _service;

    [SetUp]
    public void Setup()
    {
        _productRepositoryMock = new Mock<IProductRepository>();
        _service = new ProductService(_productRepositoryMock.Object);
    }

    [Test]
    public async Task DeleteProduct_ShouldReturnOk_WhenProductIsDeleted()
    {
        var productId = Guid.NewGuid();

        var mockedProduct = new TbProduct
        {
            ProductId = productId,
            ProductStatus = "available"
        };

        _productRepositoryMock.Setup(x => x.GetProductById(productId)).ReturnsAsync(mockedProduct);
        _productRepositoryMock.Setup(x => x.DeleteProduct(It.IsAny<TbProduct>())).Returns(Task.CompletedTask);

        await _service.DeleteProduct(productId);

        Assert.That(mockedProduct.ProductStatus, Is.EqualTo("deleted"));

        _productRepositoryMock.Verify(x => x.DeleteProduct(mockedProduct), Times.Once);
    }

    [Test]
    public void DeleteProduct_ShouldThrow_WhenProductIsNotFound()
    {
        var productId = Guid.NewGuid();

        _productRepositoryMock.Setup(x => x.GetProductById(productId)).ReturnsAsync((TbProduct)null);

        var result = Assert.ThrowsAsync<InvalidOperationException>(() => _service.DeleteProduct(productId));

        Assert.That(result.Message, Is.EqualTo("Produto não encontrado."));

        _productRepositoryMock.Verify(x => x.DeleteProduct(It.IsAny<TbProduct>()), Times.Never);
    }

    [Test]
    public void DeleteProduct_ShouldThrow_WhenProductIsAlreadyDeleted()
    {
        var productId = Guid.NewGuid();

        var mockedProduct = new TbProduct
        {
            ProductId = productId,
            ProductStatus = "deleted"
        };

        _productRepositoryMock.Setup(x => x.GetProductById(productId)).ReturnsAsync(mockedProduct);

        var result = Assert.ThrowsAsync<InvalidOperationException>(() => _service.DeleteProduct(productId));

        Assert.That(result.Message, Is.EqualTo("O produto já foi deletado."));
    }

    [Test]
    public void UpdateProduct_ShouldThrow_WhenProductIsNotFound()
    {
        var productId = Guid.NewGuid();
        var updateDto = new UpdateProductDTO { ProductValue = 100 }; 

        _productRepositoryMock.Setup(x => x.GetProductById(productId)).ReturnsAsync((TbProduct)null);

        var result = Assert.ThrowsAsync<InvalidOperationException>(() => _service.UpdateProduct(productId, updateDto));

        Assert.That(result.Message, Is.EqualTo("Produto não encontrado."));

        _productRepositoryMock.Verify(x => x.UpdateProduct(It.IsAny<TbProduct>()), Times.Never);
    }

    [Test]
    public async Task UpdateProduct_ShouldUpdateAllFields_WhenAllDataIsProvided()
    {
        var productId = Guid.NewGuid();

        var existingProduct = new TbProduct
        {
            ProductId = productId,
            ProductValue = 50m,
            ProductStatus = "available",
            ProductType = "dress",
            ProductColor = "azul",
            ProductOccasion = "casual",
            ProductSize = "m"
        };

        var updateDto = new UpdateProductDTO
        {
            ProductValue = 150m,
            ProductStatus = "unavailable",
            ProductType = "jacket",
            ProductColor = "preto",
            ProductOccasion = "wedding",
            ProductSize = "g"
        };

        _productRepositoryMock.Setup(x => x.GetProductById(productId)).ReturnsAsync(existingProduct);

        _productRepositoryMock.Setup(x => x.UpdateProduct(It.IsAny<TbProduct>())).Returns(Task.CompletedTask);

       
        await _service.UpdateProduct(productId, updateDto);

        Assert.That(existingProduct.ProductValue, Is.EqualTo(150m));
        Assert.That(existingProduct.ProductStatus, Is.EqualTo("unavailable"));
        Assert.That(existingProduct.ProductColor, Is.EqualTo("preto"));
        Assert.That(existingProduct.ProductOccasion, Is.EqualTo("wedding"));
        Assert.That(existingProduct.ProductSize, Is.EqualTo("g"));

        _productRepositoryMock.Verify(x => x.UpdateProduct(existingProduct), Times.Once);
    }

    [Test]
    public async Task UpdateProduct_ShouldUpdateOnlyProvidedFields_WhenPartialDataIsProvided()
    {
        var productId = Guid.NewGuid();

        var existingProduct = new TbProduct
        {
            ProductId = productId,
            ProductValue = 50m,
            ProductStatus = "available",
            ProductColor = "azul",
            ProductType = "dress",
            ProductOccasion = "casual",
            ProductSize = "m"
        };

        var updateDto = new UpdateProductDTO
        {
            ProductValue = 99m,
            ProductStatus = "unavailable"
        };

        _productRepositoryMock.Setup(x => x.GetProductById(productId)).ReturnsAsync(existingProduct);

        _productRepositoryMock.Setup(x => x.UpdateProduct(It.IsAny<TbProduct>())).Returns(Task.CompletedTask);

        await _service.UpdateProduct(productId, updateDto);

        Assert.That(existingProduct.ProductValue, Is.EqualTo(99m));
        Assert.That(existingProduct.ProductStatus, Is.EqualTo("unavailable"));

        
        Assert.That(existingProduct.ProductColor, Is.EqualTo("azul"));
        Assert.That(existingProduct.ProductType, Is.EqualTo("dress"));

        _productRepositoryMock.Verify(x => x.UpdateProduct(existingProduct), Times.Once);
    }
}
