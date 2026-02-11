using Closetly.DTO;
using Closetly.Models;
using Closetly.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace Closetly.Repository;

public class OrderRepository : IOrderRepository
{
    private readonly PostgresContext _context;

    public OrderRepository(PostgresContext context)
    {
        _context = context;
    }

    public TbOrder? GetOrderById(Guid id)
    {
        return _context.TbOrders.Find(id);
    }
}
