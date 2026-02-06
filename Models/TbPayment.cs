using System;
using System.Collections.Generic;

namespace Closetly.Models;

public partial class TbPayment
{
    public Guid PaymentId { get; set; }

    public Guid OrderId { get; set; }

    public string PaymentType { get; set; } = null!;

    public decimal PaymentValue { get; set; }

    public string PaymentStatus { get; set; } = null!;

    public virtual TbOrder Order { get; set; } = null!;
}
