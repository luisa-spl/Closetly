using Closetly.Models;

namespace Closetly.DTO
{
    public class RatingDTO
    {
        public Guid RatingId { get; set; }

        public Guid OrderId { get; set; }

        public DateTime? CreatedAt { get; set; }

        public int Rate { get; set; }
    }

    public class RatingCreateDTO
    {
        public Guid OrderId { get; set; }

        public int Rate { get; set; }
    }
}
