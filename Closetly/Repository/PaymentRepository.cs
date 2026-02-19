using Closetly.DTO;
using Closetly.Models;
using Closetly.Repository.Interface;

namespace Closetly.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly PostgresContext _context;

        public PaymentRepository(PostgresContext context)
        {
            _context = context;
        }


        public async Task CreatePayment(CreatePaymentDTO payment, CancellationToken ct)
        {
            _context.TbPayments.Add(new TbPayment
            {
                OrderId = payment.OrderId,
                PaymentValue = payment.PaymentValue,
                PaymentStatus = PaymentStatus.PENDING
            });
            await _context.SaveChangesAsync();
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