using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Closetly.Models
{
    [ExcludeFromCodeCoverage]
    public static class ProductStatus
    {
        public static readonly string AVAILABLE = "AVAILABLE";
        public static readonly string UNAVAILABLE = "UNAVAILABLE";
        public static readonly string DELETED = "DELETED";
    }
}