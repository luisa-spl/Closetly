using Closetly.DTO;
using Closetly.Models;

namespace Closetly.Application.Mappers;

public static class NewOrderMapper
{
    public static OrderResponseDTO MapToOrderResponseDTO(TbOrder order)
    {
        var response = new OrderResponseDTO
        {
            Id = order.OrderId,
            OrderedAt = order.OrderedAt,
            ReturnDate = order.ReturnDate,
            Total = order.OrderTotalValue,
            UserId = order.UserId,
            PaymentStatus = "PENDING",
            Products = order.TbOrderProducts.Select(p => new OrderProductResponseDTO
            {
                ProductId = p.ProductId,
                Quantity = p.Quantity,
               
            }).ToList()
        };

        return response;
    }
}
