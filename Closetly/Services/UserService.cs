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

        public string UpdateUser(Guid id, UpdateUserRequest request)
        {
            TbUser? foundUser = _repository.GetById(id);
            if (foundUser is null)
                return $"Usuário com a id {id} não encontrado.";

            if (request.Name != null)
                foundUser.UserName = request.Name;

            if (request.Phone != null)
                foundUser.Phone = request.Phone;

            if (request.Email != null)
                foundUser.Email = request.Email;

            _repository.UpdateUser(foundUser);
            return "";
        }
        
        public UserDTO? CreateUser(UserDTO user)
        {
            if(user == null)
            {
                return null;
            }

            _repository.CreateUser(user);

            return user;
        }

        public List<UserOrders>? GetUserOrders(Guid userId)
        {
            TbUser? user = _repository.GetById(userId);
            if(user == null)
            {
                return null;
            }

            return _repository.GetUserOrders(userId);
        }

        public List<UserDTO> GetUsers()
        {
            var users = _repository.GetUsers();
            return users.Select(u => new UserDTO
            {
                Id = u.UserId,
                UserName = u.UserName,
                Phone = u.Phone,
                Email = u.Email
            }).ToList();
        }
    }
}