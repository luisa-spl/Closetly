using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Closetly.Models;

[ExcludeFromCodeCoverage]
public partial class TbOrderProduct
{
    public Guid OrderId { get; set; }

    public Guid ProductId { get; set; }

    public int Quantity { get; set; }

    public virtual TbOrder Order { get; set; } = null!;

    public virtual TbProduct Product { get; set; } = null!;
}
