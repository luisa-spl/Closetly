using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Closetly.Models
{
    [ExcludeFromCodeCoverage]
    public static class PaymentStatus
    {
        public static readonly string APPROVED = "APPROVED";
        public static readonly string PENDING = "PENDING";
    }
}