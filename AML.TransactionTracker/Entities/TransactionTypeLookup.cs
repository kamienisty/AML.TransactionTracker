using System.ComponentModel.DataAnnotations;
using static AML.TransactionTracker.Core.Enums.EnumsCore;

namespace AML.TransactionTracker.Core.Entities
{
    public sealed class TransactionTypeLookup
    {
        [Key]
        public TransactionType Type { get; set; }
        public string Name { get; set; }
    }
}
