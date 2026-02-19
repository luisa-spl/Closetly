using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Closetly.DTO
{
    public class UserDTO
    {
        public Guid Id {get;set;}
        public required string UserName {get;set;}
        public required string Phone { get;set; }
        [EmailAddress(ErrorMessage = "O formato do e-mail é inválido.")]
        public required string Email { get;set; }
    }

    public class UpdateUserRequest
    {
        public string? Name {get;set;} = null;
        public string? Phone {get;set;} = null;
        public string? Email {get;set;} = null;
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