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
        public UserDTO CreateUser(UserDTO user); //cria��o de usu�rio       
        public List<UserOrders>? GetUserOrders(Guid userId);
    }
}