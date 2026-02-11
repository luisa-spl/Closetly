using System;
using System.Collections.Generic;

namespace Closetly.Models;

public partial class TbRating
{
    public Guid RatingId { get; set; }

    public Guid OrderId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int Rate { get; set; }

    public virtual TbOrder Order { get; set; } = null!;
}
