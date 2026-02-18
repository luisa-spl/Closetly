using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Closetly.DTO;

namespace Closetly.Services.Interface
{
    public interface IPaymentService
    {
        public Task CreatePayment(PaymentDTO payment, CancellationToken ct);
        public Task PayOrder(PaymentDTO payment, CancellationToken ct);
    }
}