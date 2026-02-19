using Closetly.Models;
using System.ComponentModel.DataAnnotations;

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
        [Required]
        public Guid OrderId { get; set; }
        [Required]
        [Range(1, 5, ErrorMessage = "Você deve dar uma nota de 1 a 5")]
        public int Rate { get; set; }
    }
}
