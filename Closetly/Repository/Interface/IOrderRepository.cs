using Closetly.Models;

namespace Closetly.Repository.Interface;

public interface IOrderRepository
{
    public TbOrder? GetOrderById(Guid id);
    public Task<TbOrder?> GetOrderWithProductsById(Guid id);
    public Task<TbOrder> CreateOrder(TbOrder order);
    public Task CancelOrder(TbOrder order);
    public Task UpdateOrder(TbOrder order);
    public Task<List<TbOrder>> GetOrdersByUserId(Guid userId);
}
