using Closetly.Application.Validators;
using Closetly.Application.Mappers;
using Closetly.DTO;
using Closetly.Models;
using Closetly.Repository;
using Closetly.Repository.Interface;
using Closetly.Services.Interface;
using Closetly.Services;


namespace Closetly.Services;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _repository;
    private readonly IUserRepository _userRepository;
    private readonly IProductRepository _productRepository;
    private readonly IPaymentRepository _paymentRepository;


    public OrderService
    (
        IOrderRepository orderRepository, 
        IUserRepository userRepository, 
        IProductRepository productRepository, 
        IPaymentRepository paymentRepository
    )
    {
        _repository = orderRepository;
        _userRepository = userRepository;
        _productRepository = productRepository;
        _paymentRepository = paymentRepository;
    }
 
    public async Task<OrderResponseDTO> CreateOrder(OrderRequestDTO order, CancellationToken cancellationToken)
    {
        var user = _userRepository.GetById(order.UserId);

        if (user == null)
        {
            throw new InvalidOperationException($"Usuário com Id '{order.UserId}' não encontrado");
        }

        if (order.Products.Count == 0) 
        {
            throw new InvalidOperationException($"Você deve adicionar pelo menos 1 produto ao pedido");
        }

        var dateNow = DateTime.UtcNow;

        var newOrder = new TbOrder
        {
            UserId = order.UserId,
            OrderId = Guid.NewGuid(),
            OrderedAt = dateNow,
            ReturnDate = dateNow.AddDays(order.ReturnPeriod),
            OrderStatus = OrderStatus.PENDING
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

        newOrder.TbOrderProducts = orderProducts;
        newOrder.OrderTotalItems = orderProducts.Count();
        newOrder.OrderTotalValue = total;

        var newPayment = new CreatePaymentDTO {
            PaymentValue = newOrder.OrderTotalValue,
            OrderId = newOrder.OrderId,
        };

        await OrderValidator.ChangeManyProductsStatus(_productRepository, orderProducts, ProductStatus.UNAVAILABLE);
        var createdOrder = await _repository.CreateOrder(newOrder);
        var payment = await _paymentRepository.CreatePayment(newPayment, cancellationToken);

        return NewOrderMapper.MapToOrderResponseDTO(createdOrder, payment);
    }

    public async Task CancelOrder(Guid orderId)
    {
        var order =  await _repository.GetOrderWithProductsById(orderId);

        if (order == null) {
            throw new InvalidOperationException($"Pedido com Id '{orderId}' não encontrado");
        }

        if (order.OrderStatus != "PENDING")
        {
            throw new InvalidOperationException($"Pedido com Id '{orderId}' não pode ser cancelado pois já foi pago e/ou está concluido");
        }

        order.OrderStatus = "CANCELLED";
        var orderProducts = order.TbOrderProducts.ToList();

        await OrderValidator.ChangeManyProductsStatus(_productRepository, orderProducts, "AVAILABLE");

        await _repository.CancelOrder(order);
    }
}
