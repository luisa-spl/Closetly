using Closetly.DTO;
using Closetly.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Closetly.Tests.DTO;

public class PaymentDTOTest
{
    private IList<ValidationResult> ValidateModel(object model)
    {
        var validationResults = new List<ValidationResult>();
        var ctx = new ValidationContext(model, null, null);
        Validator.TryValidateObject(model, ctx, validationResults, true);
        return validationResults;
    }

    [Test]
    public void PaymentDTO_ShouldBeValid_WhenAllDataIsCorrect()
    {       
        var dto = new PaymentDTO
        {
            OrderId = Guid.NewGuid(),
            PaymentType = PaymentType.PIX,
            PaymentValue = 150.00m
        };
        
        var results = ValidateModel(dto);
        
        Assert.That(results, Is.Empty, "O DTO deveria ser válido e não retornar erros.");
    }

    [Test]
    [TestCase(0)]
    [TestCase(-10.50)]
    public void PaymentDTO_ShouldBeInvalid_WhenPaymentValueIsZeroOrNegative(decimal invalidValue)
    {       
        var dto = new PaymentDTO
        {
            OrderId = Guid.NewGuid(),
            PaymentType = PaymentType.CREDIT,
            PaymentValue = invalidValue
        };

        var results = ValidateModel(dto);
     
        Assert.That(results, Is.Not.Empty);
        Assert.That(results.Any(v => v.ErrorMessage == "O valor do pagamento deve ser maior que zero."), Is.True);
    }

    [Test]
    public void PaymentDTO_ShouldBeInvalid_WhenPaymentTypeIsNull()
    {        
        var dto = new PaymentDTO
        {
            OrderId = Guid.NewGuid(),
            PaymentType = null!, 
            PaymentValue = 100m
        };
       
        var results = ValidateModel(dto);
      
        Assert.That(results, Is.Not.Empty);        
        Assert.That(results.Any(v => v.MemberNames.Contains("PaymentType")), Is.True);
    }

    [Test]
    public void CreatePaymentDTO_ShouldBeValid_WhenAllDataIsCorrect()
    {        
        var dto = new CreatePaymentDTO
        {
            OrderId = Guid.NewGuid(),
            PaymentValue = 50.00m
        };
       
        var results = ValidateModel(dto);
        
        Assert.That(results, Is.Empty);
    }

    [Test]
    [TestCase(0)]
    [TestCase(-5)]
    public void CreatePaymentDTO_ShouldBeInvalid_WhenPaymentValueIsZeroOrNegative(decimal invalidValue)
    {        
        var dto = new CreatePaymentDTO
        {
            OrderId = Guid.NewGuid(),
            PaymentValue = invalidValue
        };
        
        var results = ValidateModel(dto);
     
        Assert.That(results, Is.Not.Empty);        
        Assert.That(results.Any(v => v.ErrorMessage == "O valor do produto deve ser maior que zero."), Is.True);
    }
}
