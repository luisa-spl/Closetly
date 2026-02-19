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
    public void CreateProduct_ShouldThrow_WhenProductIsNull()
    {
        var result = Assert.Throws<ArgumentException>(() => _service.CreateProduct(null));
        Assert.That(result.Message, Is.EqualTo("Produto não pode ser nulo"));
        _productRepositoryMock.Verify(x => x.CreateProduct(It.IsAny<ProductDTO>()), Times.Never);
    }  

    [Test]
    public void CreateProduct_ShouldCallRepository_WhenProductIsValid()
    {
        var product = new ProductDTO
        {
            ProductId = Guid.NewGuid(),
            ProductType = ProductType.DRESS,
            ProductStatus = ProductStatus.AVAILABLE
        };

        _service.CreateProduct(product);

        _productRepositoryMock.Verify(x => x.CreateProduct(product), Times.Once);
    }

    [Test]
    public async Task DeleteProduct_ShouldReturnOk_WhenProductIsDeleted()
    {
        var productId = Guid.NewGuid();

        var mockedProduct = new TbProduct
        {
            ProductId = productId,
            ProductStatus = ProductStatus.AVAILABLE
        };

        _productRepositoryMock.Setup(x => x.GetProductById(productId)).ReturnsAsync(mockedProduct);
        _productRepositoryMock.Setup(x => x.DeleteProduct(It.IsAny<TbProduct>())).Returns(Task.CompletedTask);

        await _service.DeleteProduct(productId);

        Assert.That(mockedProduct.ProductStatus, Is.EqualTo(ProductStatus.DELETED));

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
            ProductStatus = ProductStatus.DELETED
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
            ProductStatus = ProductStatus.AVAILABLE,
            ProductType = ProductType.DRESS,
            ProductColor = "azul",
            ProductOccasion = ProductOccasion.CASUAL,
            ProductSize = ProductSize.M
        };

        var updateDto = new UpdateProductDTO
        {
            ProductValue = 150m,
            ProductStatus = ProductStatus.UNAVAILABLE,
            ProductType = ProductType.JACKET,
            ProductColor = "preto",
            ProductOccasion = ProductOccasion.WEDDING,
            ProductSize = ProductSize.L
        };

        _productRepositoryMock.Setup(x => x.GetProductById(productId)).ReturnsAsync(existingProduct);

        _productRepositoryMock.Setup(x => x.UpdateProduct(It.IsAny<TbProduct>())).Returns(Task.CompletedTask);

       
        await _service.UpdateProduct(productId, updateDto);

        Assert.That(existingProduct.ProductValue, Is.EqualTo(150m));
        Assert.That(existingProduct.ProductStatus, Is.EqualTo(ProductStatus.UNAVAILABLE));
        Assert.That(existingProduct.ProductColor, Is.EqualTo("preto"));
        Assert.That(existingProduct.ProductOccasion, Is.EqualTo(ProductOccasion.WEDDING));
        Assert.That(existingProduct.ProductSize, Is.EqualTo(ProductSize.L));

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
            ProductStatus = ProductStatus.AVAILABLE,
            ProductColor = "azul",
            ProductType = ProductType.DRESS,
            ProductOccasion = ProductOccasion.CASUAL,
            ProductSize = ProductSize.M
        };

        var updateDto = new UpdateProductDTO
        {
            ProductValue = 99m,
            ProductStatus = ProductStatus.UNAVAILABLE
        };

        _productRepositoryMock.Setup(x => x.GetProductById(productId)).ReturnsAsync(existingProduct);

        _productRepositoryMock.Setup(x => x.UpdateProduct(It.IsAny<TbProduct>())).Returns(Task.CompletedTask);

        await _service.UpdateProduct(productId, updateDto);

        Assert.That(existingProduct.ProductValue, Is.EqualTo(99m));
        Assert.That(existingProduct.ProductStatus, Is.EqualTo(ProductStatus.UNAVAILABLE));

        
        Assert.That(existingProduct.ProductColor, Is.EqualTo("azul"));
        Assert.That(existingProduct.ProductType, Is.EqualTo(ProductType.DRESS));

        _productRepositoryMock.Verify(x => x.UpdateProduct(existingProduct), Times.Once);
    }

    [Test]
    public void GetAvailableProducts_ShouldReturnListOfProducts_WhenProductsAreAvailable()
    {
        var filters = new ProductFilters { ProductType = ProductType.DRESS };

        var expectedProducts = new List<ProductDTO>
        {
            new ProductDTO { ProductId = Guid.NewGuid(), ProductType = ProductType.DRESS, ProductStatus = ProductStatus.AVAILABLE },
            new ProductDTO { ProductId = Guid.NewGuid(), ProductType = ProductType.DRESS, ProductStatus = ProductStatus.AVAILABLE }
        };

        _productRepositoryMock.Setup(x => x.GetAvailableProducts(filters)).Returns(expectedProducts);

        var result = _service.GetAvailableProducts(filters);

        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.SameAs(expectedProducts));
        _productRepositoryMock.Verify(x => x.GetAvailableProducts(filters), Times.Once);
    }
}
