using Closetly.Models;

namespace Closetly.Repository.Interface;

public interface IOrderRepository
{
    public Task<TbOrder?> GetOrderById(Guid id);
}
