using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Closetly.DTO;
using Closetly.Repository.Interface;
using Closetly.Services.Interface;

namespace Closetly.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _repository;

        public PaymentService(IPaymentRepository paymentRepository)
        {
            _repository = paymentRepository;
        }

        public async Task PayOrder(PaymentDTO payment, CancellationToken ct)
        {
            await _repository.PayOrder(payment, ct);
        }
    }
}