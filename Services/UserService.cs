using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Closetly.DTO;
using Closetly.Models;
using Closetly.Repository;
using Closetly.Services.Interface;

namespace Closetly.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository userRepository)
        {
            _repository = userRepository;
        }

        public string UpdateUser(Guid id, string newUserName, string newPhone, string newEmail)
        {
            TbUser? user = _repository.GetById(id);
            if(user == null)
            {
                return "Usuário não encontrado";
            }

            _repository.UpdateUser(id, newUserName, newPhone, newEmail);
            return "";
        }

        public List<OrderDTO>? GetUserOrders(Guid userId)
        {
            TbUser? user = _repository.GetById(userId);
            if(user == null)
            {
                return null;
            }

            List<TbOrder> orders = _repository.GetUserOrders(userId);

            List<OrderDTO> orderDTOs = orders.Select(o => new OrderDTO
            {
                OrderId = o.OrderId,
                UserId = o.UserId,
                OrderedAt = o.OrderedAt,
                ReturnDate = o.ReturnDate,
                OrderStatus = o.OrderStatus,
                OrderTotalItems = o.OrderTotalItems
            }).ToList();
            
            return orderDTOs;
        }
    }
}