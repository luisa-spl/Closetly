using System.Diagnostics.CodeAnalysis;

namespace Closetly.Models
{
    [ExcludeFromCodeCoverage]
    public static class PaymentType
    {
        public static readonly string PIX = "PIX";
        public static readonly string CREDIT = "CREDIT";
        public static readonly string DEBIT = "DEBIT";
    }
}