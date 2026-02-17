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
            Products = new List<OrderProductRequestDTO>()
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
}
