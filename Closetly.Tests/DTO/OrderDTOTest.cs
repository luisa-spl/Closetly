using Closetly.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Closetly.Tests.DTO;

internal class OrderDTOTest
{
    private IList<ValidationResult> ValidateModel(object model)
    {
        var validationResults = new List<ValidationResult>();
        var ctx = new ValidationContext(model, null, null);
        Validator.TryValidateObject(model, ctx, validationResults, true);
        return validationResults;
    }

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

    [Test]
    public void OrderRequestDTO_ShouldBeInvalid_WhenReturnPeriodIsNotAllowed()
    {
        var dto = new OrderRequestDTO
        {
            UserId = Guid.NewGuid(),
            ReturnPeriod = 5,
            Products = new List<OrderProductRequestDTO>
                {
                    new OrderProductRequestDTO { ProductId = Guid.NewGuid(), Quantity = 1 }
                }
        };
    
        var results = ValidateModel(dto);

        Assert.That(results, Is.Not.Empty);
        Assert.That(results.Any(v => v.ErrorMessage == "O período de locação deve ser de 3, 7 ou 14 dias."), Is.True);
    }

    [Test]
    public void OrderRequestDTO_ShouldBeInvalid_WhenProductsListIsEmpty()
    {
        var dto = new OrderRequestDTO
        {
            UserId = Guid.NewGuid(),
            ReturnPeriod = 7,
            Products = new List<OrderProductRequestDTO>() 
        };

        var results = ValidateModel(dto);
     
        Assert.That(results, Is.Not.Empty);
        Assert.That(results.Any(v => v.ErrorMessage == "O pedido deve conter pelo menos um produto."), Is.True);
    }

    [Test]
    public void OrderProductRequestDTO_ShouldBeInvalid_WhenQuantityIsDifferentFromOne()
    {
        var dto = new OrderProductRequestDTO
        {
            ProductId = Guid.NewGuid(),
            Quantity = 2 
        };

        var results = ValidateModel(dto);

        Assert.That(results, Is.Not.Empty);
        Assert.That(results.Any(v => v.ErrorMessage == "Você pode alugar apenas 1 unidade de cada item."), Is.True);
    }
}
