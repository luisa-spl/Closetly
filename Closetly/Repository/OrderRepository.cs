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

    public async Task<TbOrder?> GetOrderWithProductsById(Guid id)
    {
        return await _context.TbOrders
            .Include(o => o.TbOrderProducts)
                .ThenInclude(op => op.Product)
            .FirstOrDefaultAsync(o => o.OrderId == id);
    }

    public async Task<TbOrder> CreateOrder(TbOrder order)
    {
        await _context.AddAsync(order);
        await _context.SaveChangesAsync();
        return order;
    }

    public async Task UpdateOrder(TbOrder order)
    {
        _context.TbOrders.Update(order);
        await _context.SaveChangesAsync();
    }

    public async Task<List<TbOrder>> GetOrdersByUserId(Guid userId)
    {
        return await _context.TbOrders
            .Include(o => o.TbOrderProducts)
            .Where(o => o.UserId == userId)
            .OrderByDescending(o => o.OrderedAt)
            .ToListAsync();
    }
}
