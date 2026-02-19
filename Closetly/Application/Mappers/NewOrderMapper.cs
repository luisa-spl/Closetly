using Closetly.DTO;
using Closetly.Models;

namespace Closetly.Application.Mappers;

public static class NewOrderMapper
{
    public static OrderResponseDTO MapToOrderResponseDTO(TbOrder order, TbPayment payment)
    {
        var response = new OrderResponseDTO
        {
            Id = order.OrderId,
            OrderedAt = order.OrderedAt,
            ReturnDate = order.ReturnDate,
            Total = order.OrderTotalValue,
            UserId = order.UserId,
            PaymentStatus = PaymentStatus.PENDING,
            PaymentId = payment.PaymentId,
            Products = order.TbOrderProducts.Select(p => new OrderProductResponseDTO
            {
                ProductId = p.ProductId,
                Quantity = p.Quantity,
               
            }).ToList()
        };

        return response;
    }
}
