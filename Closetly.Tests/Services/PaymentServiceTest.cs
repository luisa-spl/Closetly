using Closetly.DTO;
using Closetly.Models;
using Closetly.Repository;
using Closetly.Repository.Interface;
using Closetly.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Closetly.Tests.Services;

[TestFixture]
public class PaymentServiceTest
{
    private Mock<IPaymentRepository> _paymentRepositoryMock;
    private PaymentService _service;

    [SetUp]
    public void Setup()
    {
        _paymentRepositoryMock = new Mock<IPaymentRepository>();

        _service = new PaymentService(_paymentRepositoryMock.Object);
    }

    [Test]
    public async Task PayOrder_ShouldCallRepository_WhenCalled()
    {        
        var paymentDto = new PaymentDTO { OrderId = Guid.NewGuid(), PaymentType = PaymentType.PIX, PaymentValue = 100m };
        var cancellationToken = CancellationToken.None;
                
        await _service.PayOrder(paymentDto, cancellationToken);
                
        _paymentRepositoryMock.Verify(x => x.PayOrder(paymentDto, cancellationToken), Times.Once);
    }
}
