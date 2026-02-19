using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Closetly.Models
{
    [ExcludeFromCodeCoverage]
    public static class OrderStatus
    {
        public static readonly string PENDING = "PENDING";
        public static readonly string CANCELLED = "CANCELLED";
        public static readonly string LEASED = "LEASED";
        public static readonly string CONCLUDED = "CONCLUDED";
    }
}