using Closetly.Controllers;
using Closetly.DTO;
using Closetly.Models;
using Closetly.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Closetly.Tests.Controllers;

[TestFixture]
public class PaymentControllerTest
{
    private Mock<IPaymentService> _paymentServiceMock;
    private PaymentController _controller;

    [SetUp]
    public void Setup()
    {
        _paymentServiceMock = new Mock<IPaymentService>();
        _controller = new PaymentController(_paymentServiceMock.Object);
    }

    [Test]
    public async Task PayOrder_ShouldReturn200Ok_WhenPaymentIsSuccessful()
    {
        var paymentDto = new PaymentDTO
        {
            OrderId = Guid.NewGuid(),
            PaymentType = PaymentType.PIX,
            PaymentValue = 100.00m
        };

        _paymentServiceMock.Setup(x => x.PayOrder(paymentDto, It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        var result = await _controller.PayOrder(paymentDto, CancellationToken.None);

        Assert.That(result, Is.InstanceOf<OkObjectResult>());
        var okResult = result as OkObjectResult;
        Assert.That(okResult.StatusCode, Is.EqualTo(StatusCodes.Status200OK));
        Assert.That(okResult.Value, Is.Not.Null);
    }

    [Test]
    public async Task PayOrder_ShouldReturn400BadRequest_WhenArgumentExceptionIsThrown()
    {
        var paymentDto = new PaymentDTO
        {
            OrderId = Guid.NewGuid(),
            PaymentType = PaymentType.PIX,
            PaymentValue = 100.00m
        };

        _paymentServiceMock.Setup(x => x.PayOrder(paymentDto, It.IsAny<CancellationToken>())).ThrowsAsync(new ArgumentException("Dados do pagamento inválidos."));
               
        var result = await _controller.PayOrder(paymentDto, CancellationToken.None);
                
        Assert.That(result, Is.InstanceOf<BadRequestObjectResult>());
        var badRequestResult = result as BadRequestObjectResult;
        Assert.That(badRequestResult.StatusCode, Is.EqualTo(StatusCodes.Status400BadRequest));
    }

    [Test]
    public async Task PayOrder_ShouldReturn409Conflict_WhenInvalidOperationExceptionIsThrown()
    {
        var paymentDto = new PaymentDTO
        {
            OrderId = Guid.NewGuid(),
            PaymentType = PaymentType.PIX,
            PaymentValue = 100.00m
        };

        _paymentServiceMock.Setup(x => x.PayOrder(paymentDto, It.IsAny<CancellationToken>())).ThrowsAsync(new InvalidOperationException("O pedido já está pago."));
               
        var result = await _controller.PayOrder(paymentDto, CancellationToken.None);
                
        Assert.That(result, Is.InstanceOf<ConflictObjectResult>());
        var conflictResult = result as ConflictObjectResult;
        Assert.That(conflictResult.StatusCode, Is.EqualTo(StatusCodes.Status409Conflict));
    }

    [Test]
    public async Task PayOrder_ShouldReturn500InternalServerError_WhenGenericExceptionIsThrown()
    {
        var paymentDto = new PaymentDTO
        {
            OrderId = Guid.NewGuid(),
            PaymentType = PaymentType.PIX,
            PaymentValue = 100.00m
        };
        
        _paymentServiceMock.Setup(x => x.PayOrder(paymentDto, It.IsAny<CancellationToken>())).ThrowsAsync(new Exception("Falha de conexão com o banco."));
               
        var result = await _controller.PayOrder(paymentDto, CancellationToken.None);
            
        Assert.That(result, Is.InstanceOf<ObjectResult>());
        var objectResult = result as ObjectResult;        
        Assert.That(objectResult.StatusCode, Is.EqualTo(StatusCodes.Status500InternalServerError));
    }

}
