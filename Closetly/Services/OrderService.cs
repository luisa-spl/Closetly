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

        await OrderValidator.ChangeManyProductsStatus(_productRepository, orderProducts, ProductStatus.UNAVAILABLE);
        newOrder.TbOrderProducts = orderProducts;
        newOrder.OrderTotalItems = orderProducts.Count();
        newOrder.OrderTotalValue = total;

        var createdOrder = await _repository.CreateOrder(newOrder);

        return NewOrderMapper.MapToOrderResponseDTO(createdOrder);
    }

    public async Task ReturnOrder(Guid orderId)
    {
        var order = await _repository.GetOrderWithProductsById(orderId);

        if (order == null)
        {
            throw new InvalidOperationException($"Pedido com Id '{orderId}' não encontrado");
        }

        if (order.OrderStatus == OrderStatus.CONCLUDED)
        {
            throw new InvalidOperationException($"O pedido '{orderId}' já foi devolvido");
        }

        if (order.OrderStatus == OrderStatus.CANCELLED)
        {
            throw new InvalidOperationException($"O pedido '{orderId}' está cancelado e não pode ser devolvido");
        }

        order.OrderStatus = OrderStatus.CONCLUDED;
        await _repository.UpdateOrder(order);

        foreach (var orderProduct in order.TbOrderProducts)
        {
            var product = await _productRepository.GetProductById(orderProduct.ProductId);
            if (product != null)
            {
                await _productRepository.UpdateProductStatus(product, ProductStatus.AVAILABLE);
            }
        }
    }

    public async Task<UserOrderReportDTO> GetUserOrderReport(Guid userId)
    {
        var user = _userRepository.GetById(userId);

        if (user == null)
        {
            throw new InvalidOperationException($"Usuário com Id '{userId}' não encontrado");
        }

        var orders = await _repository.GetOrdersByUserId(userId);

        var reportItems = orders.Select(o => new OrderReportItemDTO
        {
            OrderId = o.OrderId,
            OrderedAt = o.OrderedAt,
            ReturnDate = o.ReturnDate,
            OrderStatus = o.OrderStatus,
            OrderTotalItems = o.OrderTotalItems,
            OrderTotalValue = o.OrderTotalValue,
            Products = o.TbOrderProducts.Select(p => new OrderProductResponseDTO
            {
                ProductId = p.ProductId,
                Quantity = p.Quantity
            }).ToList()
        }).ToList();

        return new UserOrderReportDTO
        {
            UserId = userId,
            TotalOrders = orders.Count,
            TotalSpent = orders.Sum(o => o.OrderTotalValue),
            Orders = reportItems
        };
    }

    public async Task<string> GetUserOrderReportCsv(Guid userId)
    {
        var report = await GetUserOrderReport(userId);

        var sb = new System.Text.StringBuilder();

        // Cabeçalho
        sb.AppendLine("OrderId,OrderedAt,ReturnDate,OrderStatus,TotalItems,TotalValue,ProductIds");

        foreach (var order in report.Orders)
        {
            var productIds = string.Join("|", order.Products.Select(p => p.ProductId));
            var orderedAt = order.OrderedAt?.ToString("yyyy-MM-dd HH:mm:ss") ?? "";
            var returnDate = order.ReturnDate.ToString("yyyy-MM-dd HH:mm:ss");

            sb.AppendLine($"{order.OrderId},{orderedAt},{returnDate},{order.OrderStatus},{order.OrderTotalItems},{order.OrderTotalValue.ToString(System.Globalization.CultureInfo.InvariantCulture)},{productIds}");
        }

        // Linha de totais
        sb.AppendLine();
        sb.AppendLine($"Total de Pedidos,{report.TotalOrders}");
        sb.AppendLine($"Total Gasto,{report.TotalSpent.ToString(System.Globalization.CultureInfo.InvariantCulture)}");

        return sb.ToString();
    }
}
