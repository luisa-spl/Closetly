using Closetly.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Closetly.Tests.DTO;

public class UserDTOTest
{
    private IList<ValidationResult> ValidateModel(object model)
    {
        var validationResults = new List<ValidationResult>();
        var ctx = new ValidationContext(model, null, null);
        Validator.TryValidateObject(model, ctx, validationResults, true);
        return validationResults;
    }

    [Test]
    public void UserDTO_ShouldBeValid_WhenAllDataIsCorrect()
    {       
        var dto = new UserDTO
        {
            Id = Guid.NewGuid(),
            UserName = "Maria",
            Phone = "11999999999",
            Email = "maria@closetly.com"
        };
        
        var results = ValidateModel(dto);
        
        Assert.That(results, Is.Empty, "O DTO deveria ser válido e não retornar erros.");
    }

    [Test]
    [TestCase("luisa@")]
    [TestCase("texto-sem-arroba")]
    public void UserDTO_ShouldBeInvalid_WhenEmailFormatIsWrong(string invalidEmail)
    {     
        var dto = new UserDTO
        {
            Id = Guid.NewGuid(),
            UserName = "Luisa",
            Phone = "11999999999",
            Email = invalidEmail 
        };
      
        var results = ValidateModel(dto);
        
        Assert.That(results, Is.Not.Empty);
        Assert.That(results.Any(v => v.ErrorMessage == "O formato do e-mail é inválido."), Is.True);
    }
}
