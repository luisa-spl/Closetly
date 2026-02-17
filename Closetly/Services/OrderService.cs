using Closetly.Application.Validators;
using Closetly.Application.Mappers;
using Closetly.DTO;
using Closetly.Models;
using Closetly.Repository;
using Closetly.Repository.Interface;
using Closetly.Services.Interface;

namespace Closetly.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _repository;
    private readonly IUserRepository _userRepository;
    private readonly IProductRepository _productRepository;


    public OrderService(IOrderRepository orderRepository, IUserRepository userRepository, IProductRepository productRepository)
    {
        _repository = orderRepository;
        _userRepository = userRepository;
        _productRepository = productRepository;
    }
 
    public async Task<OrderResponseDTO> CreateOrder(OrderRequestDTO order)
    {
        var user = _userRepository.GetById(order.UserId);

        if (user == null)
        {
            throw new InvalidOperationException($"Usuário com Id '{order.UserId}' não encontrado");
        }

        var dateNow = DateTime.UtcNow;

        var newOrder = new TbOrder
        {
            UserId = order.UserId,
            OrderId = Guid.NewGuid(),
            OrderedAt = dateNow,
            ReturnDate = dateNow.AddDays(order.ReturnPeriod),
            OrderStatus = "PENDING"
        };

        decimal total = 0;
        var orderProducts = new List<TbOrderProduct>();

        foreach (var item in order.Products) 
        {
            var product = await OrderValidator.VerifyProduct(_productRepository, item);

            OrderValidator.CheckProductStatusAndQuantity(product, item.Quantity);

            total += product.ProductValue;

            orderProducts.Add(new TbOrderProduct
            {
                ProductId = product.ProductId,
                OrderId = newOrder.OrderId,
                Quantity = 1
            });
            
        }

        await OrderValidator.ChangeManyProductsStatus(_productRepository, orderProducts);
        newOrder.TbOrderProducts = orderProducts;
        newOrder.OrderTotalItems = orderProducts.Count();
        newOrder.OrderTotalValue = total;

        var createdOrder = await _repository.CreateOrder(newOrder);

        return NewOrderMapper.MapToOrderResponseDTO(createdOrder);
    }
}
