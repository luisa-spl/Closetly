using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Closetly.DTO;
using Closetly.Models;

namespace Closetly.Repository.Interface
{
    public interface IPaymentRepository
    {
        public Task PayOrder(PaymentDTO payment, CancellationToken ct);
        public Task<TbPayment?> CreatePayment(CreatePaymentDTO payment, CancellationToken ct);
    }
}