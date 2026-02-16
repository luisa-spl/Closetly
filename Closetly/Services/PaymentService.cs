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
            if (payment == null) throw new ArgumentException("Payload inválido.");
            if (payment.OrderId == Guid.Empty) throw new ArgumentException("OrderId inválido.");
            if (payment.PaymentValue <= 0) throw new ArgumentException("PaymentValue deve ser maior que zero.");

            try
            {
                await _repository.PayOrder(payment, ct);
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException)
            {
                // FK, unique constraint, etc.
                throw new InvalidOperationException("Não foi possível registrar o pagamento. Verifique o pedido e os dados informados.");
            }
        }
    }
}