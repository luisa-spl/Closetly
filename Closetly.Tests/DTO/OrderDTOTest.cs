using Closetly.DTO;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Closetly.Tests.DTO
{
    [TestFixture]
    public class OrderDTOTest
    {
        private IList<ValidationResult> ValidateModel(object model)
        {
            var validationResults = new List<ValidationResult>();
            var ctx = new ValidationContext(model, null, null);
            Validator.TryValidateObject(model, ctx, validationResults, validateAllProperties: true);
            return validationResults;
        }

        // -------------------------
        // OrderRequestDTO
        // -------------------------

        [Test]
        public void OrderRequestDTO_ShouldBeValid_WhenAllDataIsCorrect()
        {
            var dto = new OrderRequestDTO
            {
                UserId = Guid.NewGuid(),
                ReturnPeriod = 7,
                Products = new List<OrderProductRequestDTO>
                {
                    new OrderProductRequestDTO { ProductId = Guid.NewGuid(), Quantity = 1 }
                }
            };

            var results = ValidateModel(dto);

            Assert.That(results, Is.Empty, "O DTO deveria ser válido e não retornar erros.");
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(5)]
        [TestCase(10)]
        [TestCase(15)]
        public void OrderRequestDTO_ShouldBeInvalid_WhenReturnPeriodIsNotAllowed(int invalidPeriod)
        {
            var dto = new OrderRequestDTO
            {
                UserId = Guid.NewGuid(),
                ReturnPeriod = invalidPeriod,
                Products = new List<OrderProductRequestDTO>
                {
                    new OrderProductRequestDTO { ProductId = Guid.NewGuid(), Quantity = 1 }
                }
            };

            var results = ValidateModel(dto);

            Assert.That(results, Is.Not.Empty);
            Assert.That(results.Any(v => v.ErrorMessage == "O período de locação deve ser de 3, 7 ou 14 dias."), Is.True);
        }

        [TestCase(3)]
        [TestCase(7)]
        [TestCase(14)]
        public void OrderRequestDTO_ShouldBeValid_WhenReturnPeriodIsAllowed(int allowedPeriod)
        {
            var dto = new OrderRequestDTO
            {
                UserId = Guid.NewGuid(),
                ReturnPeriod = allowedPeriod,
                Products = new List<OrderProductRequestDTO>
                {
                    new OrderProductRequestDTO { ProductId = Guid.NewGuid(), Quantity = 1 }
                }
            };

            var results = ValidateModel(dto);

            Assert.That(results, Is.Empty);
        }

        [Test]
        public void OrderRequestDTO_ShouldBeInvalid_WhenProductsListIsEmpty()
        {
            var dto = new OrderRequestDTO
            {
                UserId = Guid.NewGuid(),
                ReturnPeriod = 7,
                Products = new List<OrderProductRequestDTO>() // MinLength(1)
            };

            var results = ValidateModel(dto);

            Assert.That(results, Is.Not.Empty);
            Assert.That(results.Any(v => v.ErrorMessage == "O pedido deve conter pelo menos um produto."), Is.True);
        }

        [Test]
        public void OrderRequestDTO_ShouldBeInvalid_WhenProductsIsNull()
        {
            var dto = new OrderRequestDTO
            {
                UserId = Guid.NewGuid(),
                ReturnPeriod = 7,
                Products = null! // Required
            };

            var results = ValidateModel(dto);

            Assert.That(results, Is.Not.Empty);
            Assert.That(results.Any(v => v.MemberNames.Contains(nameof(OrderRequestDTO.Products))), Is.True);
        }

        // -------------------------
        // OrderProductRequestDTO
        // -------------------------

        [Test]
        public void OrderProductRequestDTO_ShouldBeValid_WhenQuantityIsOne()
        {
            var dto = new OrderProductRequestDTO
            {
                ProductId = Guid.NewGuid(),
                Quantity = 1
            };

            var results = ValidateModel(dto);

            Assert.That(results, Is.Empty);
        }

        [TestCase(0)]
        [TestCase(2)]
        [TestCase(3)]
        [TestCase(10)]
        public void OrderProductRequestDTO_ShouldBeInvalid_WhenQuantityIsDifferentFromOne(int invalidQty)
        {
            var dto = new OrderProductRequestDTO
            {
                ProductId = Guid.NewGuid(),
                Quantity = invalidQty
            };

            var results = ValidateModel(dto);

            Assert.That(results, Is.Not.Empty);
            Assert.That(results.Any(v => v.ErrorMessage == "Você pode alugar apenas 1 unidade de cada item."), Is.True);
        }

        // -------------------------
        // OrderWithProductsRequestDTO
        // -------------------------

        [Test]
        public void OrderWithProductsRequestDTO_ShouldBeValid_WhenIdsAreValid()
        {
            var dto = new OrderWithProductsRequestDTO
            {
                UserId = Guid.NewGuid(),
                OrderId = Guid.NewGuid()
            };

            var results = ValidateModel(dto);

            Assert.That(results, Is.Empty);
        }

        [Test]
        public void OrderWithProductsRequestDTO_ShouldBeValid_EvenWhenGuidsAreEmpty_BecauseRequiredDoesNotValidateEmptyGuid()
        {

            var dto = new OrderWithProductsRequestDTO
            {
                UserId = Guid.Empty,
                OrderId = Guid.Empty
            };

            var results = ValidateModel(dto);

            Assert.That(results, Is.Empty);
        }
    }
}
