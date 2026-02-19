using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Closetly.DTO;

namespace Closetly.Repository.Interface
{
    public interface IPaymentRepository
    {
        public Task PayOrder(PaymentDTO payment, CancellationToken ct);
        public Task CreatePayment(CreatePaymentDTO payment, CancellationToken ct);
    }
}