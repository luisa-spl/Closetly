using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Closetly.DTO
{
    public class UserDTO
    {
        public Guid Id {get;set;}
        public string UserName {get;set;}
        public string Phone { get;set; }
        public string Email { get;set; }
    }

    public class UpdateUserRequest
    {
        public string Name {get;set;}
        public string Phone {get;set;}
        public string Email {get;set;}
    }
}