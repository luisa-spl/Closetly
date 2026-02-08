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

    public sealed class UserOrders
    {
        public Guid OrderId { get; set; }
        public DateTime? OrderedAt { get; set; }
        public DateTime ReturnDate { get; set; }
        public string OrderStatus { get; set; } = "";
        public int? OrderTotalItems { get; set; }

        public Guid UserId { get; set; }
        public string UserName { get; set; } = "";

        public List<OrderProduct> Products { get; set; } = new();
        public List<OrderPayment> Payments { get; set; } = new();
    }
}