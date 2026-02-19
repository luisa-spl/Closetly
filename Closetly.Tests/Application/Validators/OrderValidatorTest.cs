using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Closetly.Application.Validators;
using Closetly.DTO;
using Closetly.Models;
using Closetly.Repository.Interface;
using Moq;
using NUnit.Framework;

namespace Closetly.Tests.Application.Validators
{
    [TestFixture]
    public class OrderValidatorTests
    {
        private Mock<IProductRepository> _repoMock = null!;

        [SetUp]
        public void Setup()
        {
            _repoMock = new Mock<IProductRepository>(MockBehavior.Strict);
        }

        private static TbProduct BuildProduct(Guid id, string? status = null, string? type = null)
        {

            status ??= ProductStatus.AVAILABLE;
            type ??= ProductType.DRESS;
            return new TbProduct
            {
                ProductId = id,
                ProductStatus = status,
                ProductType = type,
                ProductColor = "BLACK",
                ProductSize = ProductSize.M,
                ProductOccasion = ProductOccasion.PARTY,
                ProductValue = 100m
            };
        }

        // -------------------------
        // VerifyProduct
        // -------------------------

        [Test]
        public async Task VerifyProduct_ShouldReturnProduct_WhenProductExists()
        {
            var productId = Guid.NewGuid();
            var orderProduct = new OrderProductRequestDTO { ProductId = productId, Quantity = 1 };
            var product = BuildProduct(productId);

            _repoMock.Setup(r => r.GetProductById(productId)).ReturnsAsync(product);

            var result = await OrderValidator.VerifyProduct(_repoMock.Object, orderProduct);

            Assert.That(result, Is.SameAs(product));
            _repoMock.Verify(r => r.GetProductById(productId), Times.Once);
        }

        [Test]
        public void VerifyProduct_ShouldThrow_WhenProductDoesNotExist()
        {
            var productId = Guid.NewGuid();
            var orderProduct = new OrderProductRequestDTO { ProductId = productId, Quantity = 1 };

            _repoMock.Setup(r => r.GetProductById(productId)).ReturnsAsync((TbProduct?)null);

            var ex = Assert.ThrowsAsync<InvalidOperationException>(
                async () => await OrderValidator.VerifyProduct(_repoMock.Object, orderProduct));

            Assert.That(ex!.Message, Is.EqualTo($"Produto com Id: '{productId}' não encontrado"));
            _repoMock.Verify(r => r.GetProductById(productId), Times.Once);
        }

        // -------------------------
        // CheckProductStatusAndQuantity
        // -------------------------

        [Test]
        public void CheckProductStatusAndQuantity_ShouldThrow_WhenProductIsNotAvailable()
        {
            var productId = Guid.NewGuid();
            var product = BuildProduct(productId, status: ProductStatus.UNAVAILABLE, type: ProductType.DRESS);

            var ex = Assert.Throws<InvalidOperationException>(
                () => OrderValidator.CheckProductStatusAndQuantity(product, itemQuantity: 1));

            Assert.That(ex!.Message, Is.EqualTo($"O produto '{product.ProductType}, com Id {product.ProductId}' não está disponível"));
        }

        [Test]
        public void CheckProductStatusAndQuantity_ShouldThrow_WhenQuantityIsZero()
        {
            var productId = Guid.NewGuid();
            var product = BuildProduct(productId, status: ProductStatus.AVAILABLE, type: ProductType.DRESS);

            var ex = Assert.Throws<InvalidOperationException>(
                () => OrderValidator.CheckProductStatusAndQuantity(product, itemQuantity: 0));

            Assert.That(ex!.Message, Is.EqualTo($"Você deve adicionar ao menos 1 unidade do produto: '{product.ProductType}, com Id {product.ProductId}' "));
        }

        [Test]
        public void CheckProductStatusAndQuantity_ShouldNotThrow_WhenAvailableAndQuantityGreaterThanZero()
        {
            var productId = Guid.NewGuid();
            var product = BuildProduct(productId, status: ProductStatus.AVAILABLE, type: ProductType.DRESS);

            Assert.DoesNotThrow(() => OrderValidator.CheckProductStatusAndQuantity(product, itemQuantity: 1));
        }

        // -------------------------
        // ChangeProductStatus
        // -------------------------

        [Test]
        public async Task ChangeProductStatus_ShouldSetStatusToUnavailable_WhenProductExists()
        {
            var productId = Guid.NewGuid();
            var orderProductParam = new TbProduct { ProductId = productId };

            var productFromRepo = BuildProduct(productId, status: ProductStatus.AVAILABLE);

            _repoMock.Setup(r => r.GetProductById(productId)).ReturnsAsync(productFromRepo);
            _repoMock.Setup(r => r.UpdateProductStatus(productFromRepo, ProductStatus.UNAVAILABLE))
                     .Returns(Task.CompletedTask);

            var result = await OrderValidator.ChangeProductStatus(_repoMock.Object, orderProductParam);

            Assert.That(result, Is.SameAs(productFromRepo));
            _repoMock.Verify(r => r.GetProductById(productId), Times.Once);
            _repoMock.Verify(r => r.UpdateProductStatus(productFromRepo, ProductStatus.UNAVAILABLE), Times.Once);
        }

        [Test]
        public void ChangeProductStatus_ShouldThrow_WhenProductDoesNotExist()
        {
            var productId = Guid.NewGuid();
            var orderProductParam = new TbProduct { ProductId = productId };

            _repoMock.Setup(r => r.GetProductById(productId)).ReturnsAsync((TbProduct?)null);

            var ex = Assert.ThrowsAsync<InvalidOperationException>(
                async () => await OrderValidator.ChangeProductStatus(_repoMock.Object, orderProductParam));

            Assert.That(ex!.Message, Is.EqualTo($"Produto com Id: '{productId}' não encontrado"));

            _repoMock.Verify(r => r.GetProductById(productId), Times.Once);
            _repoMock.Verify(r => r.UpdateProductStatus(It.IsAny<TbProduct>(), It.IsAny<string>()), Times.Never);
        }

        // -------------------------
        // ChangeManyProductsStatus
        // -------------------------

        [Test]
        public async Task ChangeManyProductsStatus_ShouldUpdateEachProductStatus_WhenAllExist()
        {
            var status = ProductStatus.UNAVAILABLE;

            var op1 = new TbOrderProduct { OrderId = Guid.NewGuid(), ProductId = Guid.NewGuid(), Quantity = 1 };
            var op2 = new TbOrderProduct { OrderId = Guid.NewGuid(), ProductId = Guid.NewGuid(), Quantity = 2 };

            var p1 = BuildProduct(op1.ProductId, status: ProductStatus.AVAILABLE, type: ProductType.DRESS);
            var p2 = BuildProduct(op2.ProductId, status: ProductStatus.AVAILABLE, type: ProductType.SHIRT);

            _repoMock.Setup(r => r.GetProductById(op1.ProductId)).ReturnsAsync(p1);
            _repoMock.Setup(r => r.GetProductById(op2.ProductId)).ReturnsAsync(p2);

            _repoMock.Setup(r => r.UpdateProductStatus(p1, status)).Returns(Task.CompletedTask);
            _repoMock.Setup(r => r.UpdateProductStatus(p2, status)).Returns(Task.CompletedTask);

            await OrderValidator.ChangeManyProductsStatus(_repoMock.Object, new List<TbOrderProduct> { op1, op2 }, status);

            _repoMock.Verify(r => r.GetProductById(op1.ProductId), Times.Once);
            _repoMock.Verify(r => r.GetProductById(op2.ProductId), Times.Once);
            _repoMock.Verify(r => r.UpdateProductStatus(p1, status), Times.Once);
            _repoMock.Verify(r => r.UpdateProductStatus(p2, status), Times.Once);
        }

        [Test]
        public void ChangeManyProductsStatus_ShouldThrow_WhenAnyProductDoesNotExist()
        {
            var status = ProductStatus.UNAVAILABLE;

            var op1 = new TbOrderProduct { OrderId = Guid.NewGuid(), ProductId = Guid.NewGuid(), Quantity = 1 };
            var op2 = new TbOrderProduct { OrderId = Guid.NewGuid(), ProductId = Guid.NewGuid(), Quantity = 2 };

            var p1 = BuildProduct(op1.ProductId, status: ProductStatus.AVAILABLE);

            _repoMock.Setup(r => r.GetProductById(op1.ProductId)).ReturnsAsync(p1);
            _repoMock.Setup(r => r.UpdateProductStatus(p1, status)).Returns(Task.CompletedTask);

            _repoMock.Setup(r => r.GetProductById(op2.ProductId)).ReturnsAsync((TbProduct?)null);

            var ex = Assert.ThrowsAsync<InvalidOperationException>(async () =>
                await OrderValidator.ChangeManyProductsStatus(_repoMock.Object, new List<TbOrderProduct> { op1, op2 }, status));

            Assert.That(ex!.Message, Is.EqualTo($"Produto com Id: '{op2.ProductId}' não encontrado"));

            _repoMock.Verify(r => r.GetProductById(op1.ProductId), Times.Once);
            _repoMock.Verify(r => r.UpdateProductStatus(p1, status), Times.Once);

            _repoMock.Verify(r => r.GetProductById(op2.ProductId), Times.Once);
            // e não deve tentar update do segundo porque ele não existe
            _repoMock.Verify(r => r.UpdateProductStatus(It.Is<TbProduct>(p => p.ProductId == op2.ProductId), status), Times.Never);
        }
    }
}
