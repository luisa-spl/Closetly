using Closetly.DTO;

namespace Closetly.Services.Interface;

public interface IOrderService
{
    public Task<OrderResponseDTO> CreateOrder(OrderRequestDTO order);
    public Task ReturnOrder(Guid orderId);
    public Task<UserOrderReportDTO> GetUserOrderReport(Guid userId); //REPORT
}
