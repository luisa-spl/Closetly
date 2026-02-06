using System;
using System.Collections.Generic;

namespace Closetly.Models;

public partial class TbOrder
{
    public Guid OrderId { get; set; }

    public Guid UserId { get; set; }

    public DateTime? OrderedAt { get; set; }

    public DateTime ReturnDate { get; set; }

    public string OrderStatus { get; set; } = null!;

    public int? OrderTotalItems { get; set; }

    public virtual ICollection<TbOrderProduct> TbOrderProducts { get; set; } = new List<TbOrderProduct>();

    public virtual ICollection<TbPayment> TbPayments { get; set; } = new List<TbPayment>();

    public virtual ICollection<TbRating> TbRatings { get; set; } = new List<TbRating>();

    public virtual TbUser User { get; set; } = null!;
}
