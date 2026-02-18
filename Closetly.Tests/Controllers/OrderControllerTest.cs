using Closetly.Controllers;
using Closetly.DTO;
using Closetly.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Closetly.Tests.Controllers;

[TestFixture]
public class OrderControllerTest
{
    private Mock<IOrderService> _orderServiceMock;
    private OrderController _controller;

    [SetUp]
    public void Setup() 
    {
        _orderServiceMock = new Mock<IOrderService>();
        _controller = new OrderController(_orderServiceMock.Object);
    }

    [Test]
    public async Task CreateOrder_ShouldReturn201Created_WhenOrderIsSuccessful()
    {
        var requestDto = new OrderRequestDTO();
        var expectedResponse = new OrderResponseDTO { Id = Guid.NewGuid() };

        _orderServiceMock
            .Setup(x => x.CreateOrder(requestDto))
            .ReturnsAsync(expectedResponse);

        var result = await _controller.CreateOrder(requestDto);

        Assert.That(result, Is.InstanceOf<ObjectResult>());
        var objectResult = result as ObjectResult;
        Assert.That(objectResult.StatusCode, Is.EqualTo(StatusCodes.Status201Created));
        Assert.That(objectResult.Value, Is.EqualTo(expectedResponse));
    }

    [Test]
    public async Task CreateOrder_ShouldReturn409Conflict_WhenProductIsUnavailable()
    {
        var requestDto = new OrderRequestDTO();
        var errorMessage = "O produto com o id especificado não está disponível para locação no momento.";

        _orderServiceMock
            .Setup(x => x.CreateOrder(requestDto))
            .ThrowsAsync(new InvalidOperationException(errorMessage));

   
        var result = await _controller.CreateOrder(requestDto);


        Assert.That(result, Is.InstanceOf<ObjectResult>());
        var objectResult = result as ObjectResult;
           
        Assert.That(objectResult.StatusCode, Is.EqualTo(StatusCodes.Status409Conflict));

        var problemDetails = objectResult.Value as ProblemDetails;
        Assert.That(problemDetails, Is.Not.Null);
        Assert.That(problemDetails.Title, Is.EqualTo("Conflito"));
        Assert.That(problemDetails.Detail, Is.EqualTo(errorMessage));
    }

    [Test]
    public async Task CancelOrder_ShouldReturn204NoContent_WhenCancelIsSuccessful()
    {
        var orderId = Guid.NewGuid();

        _orderServiceMock
            .Setup(x => x.CancelOrder(orderId))
            .Returns(Task.CompletedTask);

        var result = await _controller.CancelOrder(orderId);

        Assert.That(result, Is.InstanceOf<NoContentResult>());
    }

    [Test]
    public async Task CancelOrder_ShouldReturn404NotFound_WhenOrderIsNotFound()
    {
        var orderId = Guid.NewGuid();
        var errorMessage = $"Pedido com Id '{orderId}' não encontrado";

        _orderServiceMock
            .Setup(x => x.CancelOrder(orderId))
            .ThrowsAsync(new InvalidOperationException(errorMessage));

        var result = await _controller.CancelOrder(orderId);

        Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        var objectResult = result as NotFoundObjectResult;
        Assert.That(objectResult.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
        Assert.That(objectResult.Value, Is.EqualTo(errorMessage));
    }

    [Test]
    public async Task CancelOrder_ShouldReturn409Conflict_WhenOrderIsNotPending()
    {
        var orderId = Guid.NewGuid();
        var errorMessage = $"Pedido com Id '{orderId}' não pode ser cancelado pois já foi pago e/ou está concluido";

        _orderServiceMock
            .Setup(x => x.CancelOrder(orderId))
            .ThrowsAsync(new InvalidOperationException(errorMessage));

        var result = await _controller.CancelOrder(orderId);

        Assert.That(result, Is.InstanceOf<ConflictObjectResult>());
        var objectResult = result as ConflictObjectResult;
        Assert.That(objectResult.StatusCode, Is.EqualTo(StatusCodes.Status409Conflict));
        Assert.That(objectResult.Value, Is.EqualTo(errorMessage));
    }
}
