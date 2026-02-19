using Closetly.DTO;
using Closetly.Models;
using Closetly.Repository;
using Closetly.Repository.Interface;
using Closetly.Services;
using Moq;

namespace Closetly.Tests.Services;

[TestFixture]
public class RatingServiceTest
{
    private Mock<IRatingRepository> _ratingRepositoryMock;
    private Mock<IOrderRepository> _orderRepositoryMock;
    private RatingService _service;

    [SetUp]
    public void Setup()
    { 
        _ratingRepositoryMock = new Mock<IRatingRepository>();
        _orderRepositoryMock = new Mock<IOrderRepository>();
        _service = new RatingService(_ratingRepositoryMock.Object, _orderRepositoryMock.Object);
    }

    [Test]
    public async Task CreateRating_ShouldThrow_WhenOrderIsNotFound() 
    {
        var ratingDto = new RatingCreateDTO { OrderId = Guid.NewGuid(), Rate = 5 };

        _orderRepositoryMock.Setup(x => x.GetOrderById(It.IsAny<Guid>())).Returns((TbOrder)null);

        var ex = Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateRating(ratingDto));

        Assert.That(ex.Message, Is.EqualTo("Pedido não encontrado."));
    }

    [Test]
    public async Task CreateRating_ShouldThrow_WhenOrderStatusIsNotConcluded()
    {
        var orderId = new Guid();
        var ratingDto = new RatingCreateDTO { OrderId = orderId, Rate = 5 };

        var mockedOrder = new TbOrder
        {
            OrderId = orderId,
            OrderStatus = OrderStatus.PENDING
        };

        _orderRepositoryMock.Setup(x => x.GetOrderById(orderId)).Returns(mockedOrder);

        var ex = Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateRating(ratingDto));

        Assert.That(ex.Message, Is.EqualTo("Você só pode avaliar pedidos finalizados."));
    }

    [Test]
    public async Task CreateRating_ShouldReturnO_WhenOrderIsConcluded() 
    {
        var orderId = new Guid();
        var ratingDto = new RatingCreateDTO { OrderId = orderId, Rate = 5 };
        
        var mockedOrder = new TbOrder
        {
            OrderId = orderId,
            OrderStatus = OrderStatus.CONCLUDED
        };

        _orderRepositoryMock.Setup(x => x.GetOrderById(orderId)).Returns(mockedOrder);

        _ratingRepositoryMock.Setup(x => x.CreateRating(It.IsAny<RatingCreateDTO>())).Returns(Task.CompletedTask);

        await _service.CreateRating(ratingDto);

        _ratingRepositoryMock.Verify(x => x.CreateRating(ratingDto), Times.Once);
    }
}
