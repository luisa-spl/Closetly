using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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


        public void PayOrder(PaymentDTO payment)
        {
            _context.TbPayments.Add(new TbPayment
            {
                OrderId = payment.OrderId,
                PaymentType = payment.PaymentType,
                PaymentValue = payment.PaymentValue,
                PaymentStatus = payment.PaymentStatus
            });
            _context.SaveChanges();
        }
    }
}