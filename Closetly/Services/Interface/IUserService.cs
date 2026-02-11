using Closetly.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Closetly.Models;

namespace Closetly.Services.Interface
{
    public interface IUserService
    {
        public string UpdateUser(Guid id, string newUserName, string newPhone, string newEmail);
        public UserDTO CreateUser(UserDTO user); //criacao de usuario       
        public List<UserOrders>? GetUserOrders(Guid userId);
    }
}