 using Closetly.DTO;
using Closetly.Models;
using Closetly.Repository;
using Closetly.Repository.Interface;
using Closetly.Services.Interface;

namespace Closetly.Services
{
    public class RatingService : IRatingService
    {
        private IRatingRepository _repository;
        private readonly IOrderRepository _orderRepository;

        public RatingService(IRatingRepository ratingRepository, IOrderRepository orderRepository   )
        {
            _repository = ratingRepository;
            _orderRepository = orderRepository;
        }
        public async Task CreateRating(RatingCreateDTO rating)
        {
            var order = _orderRepository.GetOrderById(rating.OrderId);

            if (order == null)
            {
                throw new InvalidOperationException("Pedido não encontrado.");
            }

            if (order.OrderStatus != OrderStatus.CONCLUDED)
            {
                throw new InvalidOperationException("Você só pode avaliar pedidos finalizados.");
            }

            await _repository.CreateRating(rating);
        }
    }
}
