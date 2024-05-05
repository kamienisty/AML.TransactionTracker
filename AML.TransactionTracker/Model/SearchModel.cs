using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AML.TransactionTracker.Core.Model
{
    public record SearchModel
    {
        public string Name { get; }
        public string Surename { get; }
        public DateTime? Birthdate { get; }
        public string Address { get; }

        public SearchModel(string name, string surename, DateTime? birthdate, string address)
        {
            Name = name;
            Surename = surename;
            Birthdate = birthdate;
            Address = address;
        }
    }
}
