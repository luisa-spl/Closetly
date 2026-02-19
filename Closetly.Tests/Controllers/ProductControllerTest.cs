using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Closetly.Controllers;
using Closetly.DTO;
using Closetly.Models;
using Closetly.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace Closetly.Tests.Controllers
{
    [TestFixture]
    public class ProductControllerTest
    {
        private Mock<IProductService> _productServiceMock;
        private ProductController _controller;

        [SetUp]
        public void Setup()
        {
            _productServiceMock = new Mock<IProductService>();
            _controller = new ProductController(_productServiceMock.Object); 
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };
        }

        // -------------------------
        // CreateProduct
        // -------------------------

        [Test]
        public void CreateProduct_ShouldReturnOk_AndCallService()
        {
            // Arrange
            var dto = new ProductDTO
            {
                ProductId = Guid.NewGuid(),
                ProductType = ProductType.DRESS,
                ProductStatus = ProductStatus.AVAILABLE,
                ProductColor = "RED",
                ProductSize = ProductSize.M,
                ProductOccasion = ProductOccasion.PARTY,
                ProductValue = 100m
            };

            _productServiceMock.Setup(s => s.CreateProduct(dto));

            // Act
            var result = _controller.CreateProduct(dto);

            // Assert
            Assert.That(result, Is.TypeOf<OkResult>());
            var ok = (OkResult)result;
            Assert.That(ok.StatusCode, Is.EqualTo(StatusCodes.Status200OK));

            _productServiceMock.Verify(s => s.CreateProduct(dto), Times.Once);
        }

        // -------------------------
        // GetAvailableProducts
        // -------------------------

        [Test]
        public void GetAvailableProducts_ShouldReturnOk_WithProducts()
        {
            // Arrange
            var filters = new ProductFilters { ProductType = ProductType.DRESS };

            var products = new List<ProductDTO>
            {
                new ProductDTO
                {
                    ProductId = Guid.NewGuid(),
                    ProductType = ProductType.DRESS,
                    ProductStatus = ProductStatus.AVAILABLE,
                    ProductColor = "RED",
                    ProductSize = ProductSize.M,
                    ProductOccasion = ProductOccasion.PARTY,
                    ProductValue = 100m
                }
            };

            _productServiceMock.Setup(s => s.GetAvailableProducts(filters)).Returns(products);

            // Act
            var result = _controller.GetAvailableProducts(filters);

            // Assert
            var ok = result as OkObjectResult;
            Assert.That(ok, Is.Not.Null);
            Assert.That(ok!.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
            Assert.That(ok.Value, Is.SameAs(products));

            _productServiceMock.Verify(s => s.GetAvailableProducts(filters), Times.Once);
        }

        // -------------------------
        // UpdateProduct
        // -------------------------

        [Test]
        public async Task UpdateProduct_ShouldReturnNoContent_WhenServiceSucceeds()
        {
            // Arrange
            var id = Guid.NewGuid();
            var updateDto = new UpdateProductDTO();

            _productServiceMock
                .Setup(s => s.UpdateProduct(id, updateDto))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateProduct(id, updateDto);

            // Assert
            Assert.That(result, Is.TypeOf<NoContentResult>());
            var noContent = (NoContentResult)result;
            Assert.That(noContent.StatusCode, Is.EqualTo(StatusCodes.Status204NoContent));

            _productServiceMock.Verify(s => s.UpdateProduct(id, updateDto), Times.Once);
        }

        [Test]
        public async Task UpdateProduct_ShouldReturnNotFound_WhenServiceThrowsInvalidOperationException()
        {
            // Arrange
            var id = Guid.NewGuid();
            var updateDto = new UpdateProductDTO();

            _productServiceMock
                .Setup(s => s.UpdateProduct(id, updateDto))
                .ThrowsAsync(new InvalidOperationException("Produto não encontrado."));

            // Act
            var result = await _controller.UpdateProduct(id, updateDto);

            // Assert
            var notFound = result as NotFoundObjectResult;
            Assert.That(notFound, Is.Not.Null);
            Assert.That(notFound!.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
            Assert.That(notFound.Value, Is.EqualTo("Produto não encontrado."));

            _productServiceMock.Verify(s => s.UpdateProduct(id, updateDto), Times.Once);
        }

        [Test]
        public async Task UpdateProduct_ShouldReturn500WithProblemDetails_WhenServiceThrowsGenericException()
        {
            // Arrange
            var id = Guid.NewGuid();
            var updateDto = new UpdateProductDTO();

            _productServiceMock
                .Setup(s => s.UpdateProduct(id, updateDto))
                .ThrowsAsync(new Exception("Falha inesperada"));

            // Act
            var result = await _controller.UpdateProduct(id, updateDto);

            // Assert
            var obj = result as ObjectResult;
            Assert.That(obj, Is.Not.Null);
            Assert.That(obj!.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));

            var pd = obj.Value as ProblemDetails;
            Assert.That(pd, Is.Not.Null);
            Assert.That(pd!.Status, Is.EqualTo(StatusCodes.Status500InternalServerError));
            Assert.That(pd.Title, Is.EqualTo("Erro interno do servidor"));
            Assert.That(pd.Detail, Is.EqualTo("Falha inesperada"));

            _productServiceMock.Verify(s => s.UpdateProduct(id, updateDto), Times.Once);
        }

        // -------------------------
        // DeleteProduct
        // -------------------------

        [Test]
        public async Task DeleteProduct_ShouldReturnNoContent_WhenServiceSucceeds()
        {
            // Arrange
            var id = Guid.NewGuid();

            _productServiceMock
                .Setup(s => s.DeleteProduct(id))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteProduct(id);

            // Assert
            Assert.That(result, Is.TypeOf<NoContentResult>());
            var noContent = (NoContentResult)result;
            Assert.That(noContent.StatusCode, Is.EqualTo(StatusCodes.Status204NoContent));

            _productServiceMock.Verify(s => s.DeleteProduct(id), Times.Once);
        }

        [Test]
        public async Task DeleteProduct_ShouldReturnNotFound_WhenInvalidOperationContainsEncontrado()
        {
            // Arrange
            var id = Guid.NewGuid();
            var message = "Produto não encontrado."; // contém "encontrado"

            _productServiceMock
                .Setup(s => s.DeleteProduct(id))
                .ThrowsAsync(new InvalidOperationException(message));

            // Act
            var result = await _controller.DeleteProduct(id);

            // Assert
            var notFound = result as NotFoundObjectResult;
            Assert.That(notFound, Is.Not.Null);
            Assert.That(notFound!.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
            Assert.That(notFound.Value, Is.EqualTo(message));

            _productServiceMock.Verify(s => s.DeleteProduct(id), Times.Once);
        }

        [Test]
        public async Task DeleteProduct_ShouldReturnConflict_WhenInvalidOperationDoesNotContainEncontrado()
        {
            // Arrange
            var id = Guid.NewGuid();
            var message = "O produto já foi deletado."; // não contém "encontrado"

            _productServiceMock
                .Setup(s => s.DeleteProduct(id))
                .ThrowsAsync(new InvalidOperationException(message));

            // Act
            var result = await _controller.DeleteProduct(id);

            // Assert
            var conflict = result as ConflictObjectResult;
            Assert.That(conflict, Is.Not.Null);
            Assert.That(conflict!.StatusCode, Is.EqualTo(StatusCodes.Status409Conflict));
            Assert.That(conflict.Value, Is.EqualTo(message));

            _productServiceMock.Verify(s => s.DeleteProduct(id), Times.Once);
        }

        [Test]
        public async Task DeleteProduct_ShouldReturn500WithProblemDetails_WhenServiceThrowsGenericException()
        {
            // Arrange
            var id = Guid.NewGuid();

            _productServiceMock
                .Setup(s => s.DeleteProduct(id))
                .ThrowsAsync(new Exception("Falha inesperada"));

            // Act
            var result = await _controller.DeleteProduct(id);

            // Assert
            var obj = result as ObjectResult;
            Assert.That(obj, Is.Not.Null);
            Assert.That(obj!.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));

            var pd = obj.Value as ProblemDetails;
            Assert.That(pd, Is.Not.Null);
            Assert.That(pd!.Status, Is.EqualTo(StatusCodes.Status500InternalServerError));
            Assert.That(pd.Title, Is.EqualTo("Erro interno do servidor"));
            Assert.That(pd.Detail, Is.EqualTo("Falha inesperada"));

            _productServiceMock.Verify(s => s.DeleteProduct(id), Times.Once);
        }

    }
}