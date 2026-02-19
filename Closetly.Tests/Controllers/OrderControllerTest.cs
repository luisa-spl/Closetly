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
            .Setup(x => x.CreateOrder(requestDto, It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedResponse);

        var result = await _controller.CreateOrder(requestDto, CancellationToken.None);

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
            .Setup(x => x.CreateOrder(requestDto, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new InvalidOperationException(errorMessage));

   
        var result = await _controller.CreateOrder(requestDto, CancellationToken.None);


        Assert.That(result, Is.InstanceOf<ObjectResult>());
        var objectResult = result as ObjectResult;
           
        Assert.That(objectResult.StatusCode, Is.EqualTo(StatusCodes.Status409Conflict));

        var problemDetails = objectResult.Value as ProblemDetails;
        Assert.That(problemDetails, Is.Not.Null);
        Assert.That(problemDetails.Title, Is.EqualTo("Conflito"));
        Assert.That(problemDetails.Detail, Is.EqualTo(errorMessage));
    }

    //CANCEL ORDER

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

    //RETURN ORDER

    [Test]
    public async Task ReturnOrder_ShouldReturn204NoContent_WhenReturnIsSuccessful()
    {        
        var orderId = Guid.NewGuid();

        _orderServiceMock.Setup(x => x.ReturnOrder(orderId)).Returns(Task.CompletedTask);
      
        var result = await _controller.ReturnOrder(orderId);
       
        Assert.That(result, Is.InstanceOf<NoContentResult>());
    }

    [Test]
    public async Task ReturnOrder_ShouldReturn404NotFound_WhenOrderDoesNotExist()
    {        
        var orderId = Guid.NewGuid();
        var errorMessage = $"Pedido com Id '{orderId}' não encontrado";
           
        _orderServiceMock.Setup(x => x.ReturnOrder(orderId)).ThrowsAsync(new InvalidOperationException(errorMessage));

        var result = await _controller.ReturnOrder(orderId);
       
        Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        var objectResult = result as NotFoundObjectResult;
       
        Assert.That(objectResult.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));
      
        var problemDetails = objectResult.Value as ProblemDetails;
        Assert.That(problemDetails, Is.Not.Null);
        Assert.That(problemDetails.Title, Is.EqualTo("Não Encontrado"));
        Assert.That(problemDetails.Detail, Is.EqualTo(errorMessage));
    }

    [Test]
    public async Task ReturnOrder_ShouldReturn400BadRequest_WhenOrderCannotBeReturned()
    {       
        var orderId = Guid.NewGuid();        
        var errorMessage = $"O pedido '{orderId}' já foi devolvido";

             _orderServiceMock
            .Setup(x => x.ReturnOrder(orderId)).ThrowsAsync(new InvalidOperationException(errorMessage));

       var result = await _controller.ReturnOrder(orderId);
       
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        var objectResult = result as BadRequestObjectResult;
       
        Assert.That(objectResult.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));

        var problemDetails = objectResult.Value as ProblemDetails;
        Assert.That(problemDetails, Is.Not.Null);
        Assert.That(problemDetails.Title, Is.EqualTo("Solicitação Inválida"));
        Assert.That(problemDetails.Detail, Is.EqualTo(errorMessage));
    }

    //GET USER ORDER REPORT

    [Test]
    public async Task GetUserOrderReport_ShouldReturn200Ok_WhenReportIsGenerated()
    {        
        var userId = Guid.NewGuid();
        var expectedReport = new UserOrderReportDTO
        {
            UserId = userId,
            TotalOrders = 1,
            TotalSpent = 100m
        };

        _orderServiceMock.Setup(x => x.GetUserOrderReport(userId)).ReturnsAsync(expectedReport);
             
        var result = await _controller.GetUserOrderReport(userId);
        
        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = result as OkObjectResult;
        Assert.That(okResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        Assert.That(okResult.Value, Is.EqualTo(expectedReport));
    }

    [Test]
    public async Task GetUserOrderReport_ShouldReturn404NotFound_WhenUserDoesNotExist()
    {        
        var userId = Guid.NewGuid();
        var errorMessage = $"Usuário com Id '{userId}' não encontrado";
               
        _orderServiceMock.Setup(x => x.GetUserOrderReport(userId)).ThrowsAsync(new InvalidOperationException(errorMessage));

        var result = await _controller.GetUserOrderReport(userId);

        Assert.That(result, Is.InstanceOf<NotFoundObjectResult>());
        var notFoundResult = result as NotFoundObjectResult;
        Assert.That(notFoundResult.StatusCode, Is.EqualTo(StatusCodes.Status404NotFound));

        var problemDetails = notFoundResult.Value as ProblemDetails;
        Assert.That(problemDetails, Is.Not.Null);
        Assert.That(problemDetails.Title, Is.EqualTo("Não Encontrado"));
        Assert.That(problemDetails.Detail, Is.EqualTo(errorMessage));
    }

    [Test]
    public async Task GetUserOrderReport_ShouldReturn400BadRequest_WhenOtherInvalidOperationOccurs()
    {        
        var userId = Guid.NewGuid();
        var errorMessage = "Erro inesperado na geração do relatório.";
                
        _orderServiceMock.Setup(x => x.GetUserOrderReport(userId)).ThrowsAsync(new InvalidOperationException(errorMessage));

        var result = await _controller.GetUserOrderReport(userId);

        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        var badRequestResult = result as BadRequestObjectResult;
        Assert.That(badRequestResult.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));

        var problemDetails = badRequestResult.Value as ProblemDetails;
        Assert.That(problemDetails, Is.Not.Null);
        Assert.That(problemDetails.Title, Is.EqualTo("Solicitação Inválida"));
    }
}
