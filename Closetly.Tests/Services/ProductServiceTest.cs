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
}
