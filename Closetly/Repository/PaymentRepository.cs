using Closetly.DTO;
using Closetly.Models;
using Closetly.Repository.Interface;
using System.Diagnostics.CodeAnalysis;

namespace Closetly.Repository
{
    [ExcludeFromCodeCoverage]
    public class PaymentRepository : IPaymentRepository
    {
        private readonly PostgresContext _context;

        public PaymentRepository(PostgresContext context)
        {
            _context = context;
        }

        public async Task<TbPayment?> CreatePayment(CreatePaymentDTO payment, CancellationToken ct)
        {
            var newPayment = new TbPayment
            {
                PaymentId = new Guid(),
                OrderId = payment.OrderId,
                PaymentStatus = PaymentStatus.PENDING,
                PaymentValue = payment.PaymentValue,
                PaymentType = PaymentType.PIX,

            };

            _context.TbPayments.Add(newPayment);
            await _context.SaveChangesAsync(ct);
            return newPayment;
        }

        public async Task PayOrder(PaymentDTO payment, CancellationToken ct)
        {
            var order = _context.TbOrders.Find(payment.OrderId);

            if (order == null)
                throw new Exception("Pedido não encontrado.");

            order.OrderStatus = OrderStatus.LEASED;

            var updatePayment = _context.TbPayments.FirstOrDefault(p => p.OrderId == payment.OrderId);
            
            if (updatePayment != null)
            {
                updatePayment.PaymentStatus = PaymentStatus.APPROVED;
            }
            await _context.SaveChangesAsync(ct);

        }
    }
}