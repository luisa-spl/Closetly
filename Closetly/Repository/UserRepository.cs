using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Closetly.DTO;
using Closetly.Models;
using Microsoft.EntityFrameworkCore;

namespace Closetly.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly PostgresContext _context;

        public UserRepository(PostgresContext context)
        {
            _context = context;
        }

        public TbUser? GetById(Guid id)
        {
            TbUser? user = _context.TbUsers.Find(id);
            return user;
        }

        public void UpdateUser(TbUser user)
        {
            _context.TbUsers.Update(user);
            _context.SaveChanges();
        }

        public List<UserOrders> GetUserOrders(Guid userId)
        {
            var orders = _context.TbOrders
                    .AsNoTracking()
                    .Where(o => o.UserId == userId)
                    .OrderByDescending(o => o.OrderedAt)
                    .Select(o => new UserOrders
                    {
                        OrderId = o.OrderId,
                        OrderedAt = o.OrderedAt,
                        ReturnDate = o.ReturnDate,
                        OrderStatus = o.OrderStatus,
                        OrderTotalItems = o.OrderTotalItems,

                        UserId = o.UserId,
                        UserName = o.User.UserName,

                        Products = o.TbOrderProducts.Select(op => new OrderProduct
                        {
                            ProductId = op.ProductId,
                            ProductType = op.Product.ProductType,
                            Quantity = op.Quantity
                        }).ToList(),

                        Payments = o.TbPayments.Select(p => new OrderPayment
                        {
                            PaymentId = p.PaymentId,
                            PaymentType = p.PaymentType,
                            PaymentValue = p.PaymentValue,
                            PaymentStatus = p.PaymentStatus
                        }).ToList()
                    })
                    .ToList();

            return orders;
        }

        public void CreateUser (UserDTO user)
        {
            TbUser newUser = new TbUser();

            newUser.UserName = user.UserName;
            newUser.Phone = user.Phone;
            newUser.Email = user.Email;

            _context.TbUsers.Add(newUser);
            _context.SaveChanges();
        }

        public List<TbUser> GetUsers()
        {
            return _context.TbUsers.ToList();
        }
        
    }
}