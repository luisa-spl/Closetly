using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Closetly.DTO;

namespace Closetly.Services.Interface
{
    public interface IPaymentService
    {
        public void PayOrder(PaymentDTO payment);
    }
}