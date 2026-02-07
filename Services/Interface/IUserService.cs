using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Closetly.DTO;
using Closetly.Models;

namespace Closetly.Services.Interface
{
    public interface IUserService
    {
        
        public string UpdateUser(Guid id, string newUserName, string newPhone, string newEmail);
        public List<OrderDTO>? GetUserOrders(Guid userId);

    }
}