using Closetly.DTO;
using Closetly.Models;
using Closetly.Repository.Interface;
using Closetly.Repository;
using Closetly.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace Closetly.Tests.Services;

[TestFixture]
internal class OrderServiceTest
{
    private Mock<IOrderRepository> _orderRepositoryMock;
    private Mock<IUserRepository> _userRepositoryMock;
    private Mock<IProductRepository> _productRepositoryMock;
    private OrderService _service;

    [SetUp]

    public void Setup() 
    {
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _productRepositoryMock = new Mock<IProductRepository>();

        _service = new OrderService(_orderRepositoryMock.Object, _userRepositoryMock.Object, _productRepositoryMock.Object);
    }

    [Test]
    public void CreateOrder_ShouldThrowException_WhenUserIsNotFound()
    {
        var requestDto = new OrderRequestDTO
        {
            UserId = Guid.NewGuid(),
            ReturnPeriod = 3,
            Products = new List<OrderProductRequestDTO>
            {
                new OrderProductRequestDTO { ProductId = Guid.NewGuid(), Quantity = 1 }
            }
        };

        _userRepositoryMock
            .Setup(x => x.GetById(requestDto.UserId))
            .Returns((TbUser)null);

        var result = Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.CreateOrder(requestDto)
        );

        Assert.That(result.Message, Does.Contain($"Usuário com Id '{requestDto.UserId}' não encontrado"));

        _orderRepositoryMock.Verify(x => x.CreateOrder(It.IsAny<TbOrder>()), Times.Never);
    }

    [Test]
    public void CreateOrder_ShouldThrowException_WhenProductsListIsEmpty()
    {
        var userId = Guid.NewGuid();
        var requestDto = new OrderRequestDTO
        {
            UserId = userId,
            ReturnPeriod = 3,
            Products = new List<OrderProductRequestDTO>()
        };

        var validUser = new TbUser { UserId = userId };

        _userRepositoryMock.Setup(x => x.GetById(userId)).Returns(validUser);

        var ex = Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateOrder(requestDto));

        Assert.That(ex.Message, Is.EqualTo("Você deve adicionar pelo menos 1 produto ao pedido"));

        _orderRepositoryMock.Verify(x => x.CreateOrder(It.IsAny<TbOrder>()), Times.Never);
    }

    [Test]
    public async Task CreateOrder_ShouldReturnOrderResponseDTO_WhenAllDataIsValid()
    {
        var userId = Guid.NewGuid();
        var productId = Guid.NewGuid();

        var requestDto = new OrderRequestDTO
        {
            UserId = userId,
            ReturnPeriod = 7, 
            Products = new List<OrderProductRequestDTO>
        {
            new OrderProductRequestDTO { ProductId = productId, Quantity = 1 }
        }
        };
                
        var validUser = new TbUser { UserId = userId };
        _userRepositoryMock
            .Setup(x => x.GetById(userId))
            .Returns(validUser);

        var validProduct = new TbProduct
        {
            ProductId = productId,
            ProductStatus = "AVAILABLE",
            ProductValue = 150.00m
        };

        _productRepositoryMock
            .Setup(x => x.GetProductById(productId))
            .ReturnsAsync(validProduct);

        var createdOrderFromDb = new TbOrder
        {
            OrderId = Guid.NewGuid(),
            UserId = userId,
            OrderStatus = "PENDING",
            OrderTotalValue = 150.00m,
            OrderTotalItems = 1
        };

        _orderRepositoryMock
            .Setup(x => x.CreateOrder(It.IsAny<TbOrder>()))
            .ReturnsAsync(createdOrderFromDb);

        var result = await _service.CreateOrder(requestDto);
              
        Assert.That(result, Is.Not.Null);
              
        _orderRepositoryMock.Verify(x => x.CreateOrder(It.Is<TbOrder>(orderParaSalvar =>
            orderParaSalvar.UserId == userId &&
            orderParaSalvar.OrderTotalItems == 1 &&
            orderParaSalvar.OrderTotalValue == 150.00m && 
            orderParaSalvar.OrderStatus == "PENDING"
        )), Times.Once);
    }

    [Test]
    public void CancelOrder_ShouldThrowException_WhenOrderIsNotFound()
    {
        var orderId = Guid.NewGuid();

        _orderRepositoryMock.Setup(x => x.GetOrderWithProductsById(orderId)).ReturnsAsync((TbOrder)null);

        var ex = Assert.ThrowsAsync<InvalidOperationException>(() => _service.CancelOrder(orderId));

        Assert.That(ex.Message, Does.Contain($"Pedido com Id '{orderId}' não encontrado"));
       
        _orderRepositoryMock.Verify(x => x.CancelOrder(It.IsAny<TbOrder>()), Times.Never);
    }

    [Test]
    public void CancelOrder_ShouldThrowException_WhenOrderStatusIsNotPending()
    {
        var orderId = Guid.NewGuid();
        var mockOrder = new TbOrder
        {
            OrderId = orderId,
            OrderStatus = "LEASED"
        };

        _orderRepositoryMock.Setup(x => x.GetOrderWithProductsById(orderId)).ReturnsAsync(mockOrder);

        var ex = Assert.ThrowsAsync<InvalidOperationException>(() => _service.CancelOrder(orderId));

        Assert.That(ex.Message, Does.Contain("não pode ser cancelado pois já foi pago"));
        _orderRepositoryMock.Verify(x => x.CancelOrder(It.IsAny<TbOrder>()), Times.Never);
    }

    [Test]
    public async Task CancelOrder_ShouldCancelOrder_WhenOrderIsValidAndPending()
    {
        var orderId = Guid.NewGuid();
        var product1Id = Guid.NewGuid();
        var product2Id = Guid.NewGuid();

        var product1 = new TbProduct { ProductId = product1Id, ProductStatus = "UNAVAILABLE" };
        var product2 = new TbProduct { ProductId = product2Id, ProductStatus = "UNAVAILABLE" };

        _productRepositoryMock
        .Setup(x => x.GetProductById(product1Id))
        .ReturnsAsync(product1);

        _productRepositoryMock
            .Setup(x => x.GetProductById(product2Id))
            .ReturnsAsync(product2);

        var mockOrder = new TbOrder
        {
            OrderId = orderId,
            OrderStatus = "PENDING",
            TbOrderProducts = new List<TbOrderProduct>
            {
                new TbOrderProduct { OrderId= orderId, ProductId = product1Id, Quantity = 1 },
                new TbOrderProduct { OrderId= orderId, ProductId = product2Id, Quantity = 1 }
            }
        };

        _orderRepositoryMock.Setup(x => x.GetOrderWithProductsById(orderId)).ReturnsAsync(mockOrder);

        await _service.CancelOrder(orderId);

        Assert.That(mockOrder.OrderStatus, Is.EqualTo("CANCELLED"));

        _orderRepositoryMock.Verify(x => x.CancelOrder(mockOrder), Times.Once);
    }
}
