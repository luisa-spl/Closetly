using Closetly.DTO;
using Closetly.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Closetly.Repository
{
    public interface IUserRepository
    {
        public void CreateUser(TbUser user);
        public TbUser? GetById(Guid id);
        public List<UserOrders> GetUserOrders(Guid userId);
        public void UpdateUser(TbUser user);
        public List<TbUser> GetUsers();
    }
}