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
            TbUser? findedUser = _context.TbUsers.Find(id);
            findedUser.UserName = newUserName;
            findedUser.Phone = newPhone;
            findedUser.Email = newEmail;
            _context.SaveChanges();
        }
        
    }
}