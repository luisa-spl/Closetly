using Closetly.Models;

namespace Closetly.Repository.Interface;

public interface IOrderRepository
{
    public TbOrder? GetOrderById(Guid id);
}
