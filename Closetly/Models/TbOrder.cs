using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Closetly.Models;

[ExcludeFromCodeCoverage]
public partial class TbOrder
{
    public Guid OrderId { get; set; }

    public Guid UserId { get; set; }

    public DateTime? OrderedAt { get; set; }

    public DateTime ReturnDate { get; set; }

    public string OrderStatus { get; set; } = null!;

    public int? OrderTotalItems { get; set; }

    [Column("order_total_value")]
    public decimal OrderTotalValue { get; set; }

    public virtual ICollection<TbOrderProduct> TbOrderProducts { get; set; } = new List<TbOrderProduct>();

    public virtual ICollection<TbPayment> TbPayments { get; set; } = new List<TbPayment>();

    public virtual ICollection<TbRating> TbRatings { get; set; } = new List<TbRating>();

    public virtual TbUser User { get; set; } = null!;
}
