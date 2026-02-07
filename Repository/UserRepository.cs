using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public void UpdateUser(Guid id, string newUserName, string newPhone, string newEmail)
        {
            TbUser? foundUser = _context.TbUsers.Find(id);
            if (foundUser is null)
                throw new InvalidOperationException($"User with id {id} not found.");

            foundUser.UserName = newUserName;
            foundUser.Phone = newPhone;
            foundUser.Email = newEmail;
            _context.SaveChanges();
        }

        public List<TbOrder> GetUserOrders(Guid userId)
        {
            var orders = _context.TbOrders.Where(o => o.UserId == userId).ToList();
            
            return orders;
        }
        
    }
}