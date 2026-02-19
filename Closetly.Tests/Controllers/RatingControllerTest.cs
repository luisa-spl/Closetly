using Closetly.Controllers;
using Closetly.DTO;
using Closetly.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Closetly.Tests.Controllers;

[TestFixture]
public class RatingControllerTest
{
    private Mock<IRatingService> _ratingServiceMock;
    private RatingController _controller;

    [SetUp]
    public void Setup()
    {
        _ratingServiceMock = new Mock<IRatingService>();
        _controller = new RatingController(_ratingServiceMock.Object);
    }

    [Test]
    public async Task CreateRating_ShouldReturn200Ok_WhenRatingIsSuccessful()
    {       
        var ratingDto = new RatingCreateDTO
        {
            OrderId = Guid.NewGuid(),
            Rate = 5
        };
        
        _ratingServiceMock.Setup(x => x.CreateRating(ratingDto)).Returns(Task.CompletedTask);
                
        var result = await _controller.CreateRating(ratingDto);
                
        Assert.That(result, Is.InstanceOf<OkResult>());
    }

    [Test]
    public async Task CreateRating_ShouldReturn404NotFound_WhenExceptionContainsEncontrado()
    {        
        var ratingDto = new RatingCreateDTO { OrderId = Guid.NewGuid(), Rate = 5 };
        var errorMessage = "Pedido com o id fornecido não foi encontrado.";
                
        _ratingServiceMock.Setup(x => x.CreateRating(ratingDto)).ThrowsAsync(new InvalidOperationException(errorMessage));
               
        var result = await _controller.CreateRating(ratingDto);
                
        Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        var notFoundResult = result as NotFoundObjectResult;
        Assert.That(notFoundResult.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
                
        Assert.That(notFoundResult.Value, Is.EqualTo(errorMessage));
    }

    [Test]
    public async Task CreateRating_ShouldReturn400BadRequest_WhenExceptionDoesNotContainEncontrado()
    {        
        var ratingDto = new RatingCreateDTO { OrderId = Guid.NewGuid(), Rate = 5 };       
        var errorMessage = "O pedido não pode ser avaliado no status atual.";

        _ratingServiceMock.Setup(x => x.CreateRating(ratingDto)).ThrowsAsync(new InvalidOperationException(errorMessage));

        var result = await _controller.CreateRating(ratingDto);
                
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        var badRequestResult = result as BadRequestObjectResult;
        Assert.That(badRequestResult.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
        Assert.That(badRequestResult.Value, Is.EqualTo(errorMessage));
    }
}
