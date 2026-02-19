using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Closetly.Models;

[ExcludeFromCodeCoverage]
public partial class TbUser
{
    public Guid UserId { get; set; }

    public string UserName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public virtual ICollection<TbOrder> TbOrders { get; set; } = new List<TbOrder>();
}
