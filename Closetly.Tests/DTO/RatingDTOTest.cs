using Closetly.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Closetly.Tests.DTO;

public class RatingDTOTest
{
    private IList<ValidationResult> ValidateModel(object model)
    {
        var validationResults = new List<ValidationResult>();
        var ctx = new ValidationContext(model, null, null);
        Validator.TryValidateObject(model, ctx, validationResults, true);
        return validationResults;
    }

    [Test]
    [TestCase(1)]
    [TestCase(3)]
    [TestCase(5)]
    public void RatingCreateDTO_ShouldBeValid_WhenRateIsBetween1And5(int validValue)
    {       
        var dto = new RatingCreateDTO
        {
            OrderId = Guid.NewGuid(),
            Rate = validValue
        };
      
        var results = ValidateModel(dto);
     
        Assert.That(results, Is.Empty, $"O DTO deveria ser válido para a nota {validValue}.");
    }

    [Test]
    public void RatingCreateDTO_ShouldBeInvalid_WhenRateIsLessThan1()
    {        
        var dto = new RatingCreateDTO
        {
            OrderId = Guid.NewGuid(),
            Rate = 0 
        };
        
        var results = ValidateModel(dto);
        
        Assert.That(results, Is.Not.Empty);
        Assert.That(results.Any(v => v.ErrorMessage == "Você deve dar uma nota de 1 a 5"), Is.True);
    }

    [Test]
    public void RatingCreateDTO_ShouldBeInvalid_WhenRateIsGreaterThan5()
    {        
        var dto = new RatingCreateDTO
        {
            OrderId = Guid.NewGuid(),
            Rate = 6 
        };
      
        var results = ValidateModel(dto);
        
        Assert.That(results, Is.Not.Empty);
        Assert.That(results.Any(v => v.ErrorMessage == "Você deve dar uma nota de 1 a 5"), Is.True);
    }
}
