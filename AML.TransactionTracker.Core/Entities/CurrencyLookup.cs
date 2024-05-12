using System.ComponentModel.DataAnnotations;
using static AML.TransactionTracker.Core.Enums.EnumsCore;

namespace AML.TransactionTracker.Core.Entities
{
    public sealed class CurrencyLookup
    {
        [Key]
        public Currency Currency { get; set; }
        public string Name { get; set; }
    }
}
