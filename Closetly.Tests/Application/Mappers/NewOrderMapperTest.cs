using System;
using System.Collections.Generic;
using System.Linq;
using Closetly.Application.Mappers;
using Closetly.DTO;
using Closetly.Models;
using NUnit.Framework;

namespace Closetly.Tests.Application.Mappers
{
    [TestFixture]
    public class NewOrderMapperTests
    {
        [Test]
        public void MapToOrderResponseDTO_ShouldMapAllFieldsCorrectly()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var paymentId = Guid.NewGuid();

            var orderedAt = new DateTime(2026, 02, 19, 12, 0, 0, DateTimeKind.Utc);
            var returnDate = orderedAt.AddDays(7);

            var order = new TbOrder
            {
                OrderId = orderId,
                UserId = userId,
                OrderedAt = orderedAt,
                ReturnDate = returnDate,
                OrderTotalValue = 250.50m,
                TbOrderProducts = new List<TbOrderProduct>
                {
                    new TbOrderProduct { OrderId = orderId, ProductId = Guid.NewGuid(), Quantity = 1 },
                    new TbOrderProduct { OrderId = orderId, ProductId = Guid.NewGuid(), Quantity = 2 }
                }
            };

            var payment = new TbPayment
            {
                PaymentId = paymentId
            };

            // Act
            var result = NewOrderMapper.MapToOrderResponseDTO(order, payment);

            // Assert - campos simples
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(orderId));
            Assert.That(result.UserId, Is.EqualTo(userId));
            Assert.That(result.OrderedAt, Is.EqualTo(orderedAt));
            Assert.That(result.ReturnDate, Is.EqualTo(returnDate));
            Assert.That(result.Total, Is.EqualTo(order.OrderTotalValue));
            Assert.That(result.PaymentId, Is.EqualTo(paymentId));

            // PaymentStatus fixo no mapper
            Assert.That(result.PaymentStatus, Is.EqualTo(PaymentStatus.PENDING));

            // Assert - Products
            Assert.That(result.Products, Is.Not.Null);
            Assert.That(result.Products.Count, Is.EqualTo(2));

            // garante que mapeou os mesmos ProductId e Quantity
            var expected = order.TbOrderProducts
                .Select(p => (p.ProductId, p.Quantity))
                .ToHashSet();

            var actual = result.Products
                .Select(p => (p.ProductId, p.Quantity))
                .ToHashSet();

            Assert.That(actual.SetEquals(expected), Is.True);
        }
    }
}
