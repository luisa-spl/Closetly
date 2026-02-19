using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Closetly.Models;

[ExcludeFromCodeCoverage]
public partial class TbProduct
{
    public Guid ProductId { get; set; }

    public string ProductColor { get; set; } = null!;

    public string ProductSize { get; set; } = null!;

    public string ProductType { get; set; } = null!;

    public string ProductOccasion { get; set; } = null!;

    public string ProductStatus { get; set; } = null!;

    public decimal ProductValue { get; set; }

    public virtual ICollection<TbOrderProduct> TbOrderProducts { get; set; } = new List<TbOrderProduct>();
}
