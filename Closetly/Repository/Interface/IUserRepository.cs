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
        public void CreateUser(UserDTO user);
        public TbUser? GetById(Guid id);
        public List<UserOrders> GetUserOrders(Guid userId);
        public void UpdateUser(Guid id, string newUserName, string newPhone, string newEmail);
        public List<TbUser> GetUsers();
    }
}