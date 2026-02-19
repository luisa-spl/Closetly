using Closetly.DTO;
using Closetly.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Closetly.Tests.DTO;

public class ProductDTOTest
{
    private IList<ValidationResult> ValidateModel(object model)
    {
        var validationResults = new List<ValidationResult>();
        var ctx = new ValidationContext(model, null, null);
        Validator.TryValidateObject(model, ctx, validationResults, true);
        return validationResults;
    }

    [Test]
    public void ProductDTO_ShouldBeValid_WhenAllDataIsCorrect()
    {
        var dto = new ProductDTO
        {
            ProductId = Guid.NewGuid(),
            ProductColor = "Preto",
            ProductSize = ProductSize.L,
            ProductType = ProductType.DRESS,
            ProductOccasion = ProductOccasion.PARTY,
            ProductStatus = ProductStatus.AVAILABLE,
            ProductValue = 150.50m
        };

        var results = ValidateModel(dto);

        Assert.That(results, Is.Empty, "O DTO deveria ser válido e não retornar erros.");
    }

    [Test]
    public void ProductDTO_ShouldBeInvalid_WhenProductValueIsZeroOrNegative()
    {
        var dto = new ProductDTO
        {
            ProductId = Guid.NewGuid(),
            ProductColor = "Preto",
            ProductSize = ProductSize.L,
            ProductType = ProductType.DRESS,
            ProductOccasion = ProductOccasion.PARTY,
            ProductStatus = ProductStatus.AVAILABLE,
            ProductValue = 0m
        };

        var results = ValidateModel(dto);

        Assert.That(results, Is.Not.Empty);
        Assert.That(results.Any(v => v.ErrorMessage == "O valor do produto deve ser maior que zero."), Is.True);
    }

    [Test]
    public void ProductDTO_ShouldBeInvalid_WhenRequiredStringIsMissing()
    {
        var dto = new ProductDTO
        {
            ProductId = Guid.NewGuid(),
            ProductColor = null!,
            ProductSize = ProductSize.L,
            ProductType = ProductType.DRESS,
            ProductOccasion = ProductOccasion.PARTY,
            ProductStatus = ProductStatus.AVAILABLE,
            ProductValue = 100m
        };
        
        var results = ValidateModel(dto);
        
        Assert.That(results, Is.Not.Empty);
        Assert.That(results.Any(v => v.ErrorMessage == "A cor do produto é obrigatória."), Is.True);
    }
}

