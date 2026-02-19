using Closetly.DTO;

namespace Closetly.Services.Interface;

public interface IOrderService
{
    public Task ReturnOrder(Guid orderId);
    public Task<UserOrderReportDTO> GetUserOrderReport(Guid userId);
    public Task<string> GetUserOrderReportCsv(Guid userId);
    public Task<OrderResponseDTO> CreateOrder(OrderRequestDTO order, CancellationToken cancellationToken);
    public Task CancelOrder(Guid orderId);
}
