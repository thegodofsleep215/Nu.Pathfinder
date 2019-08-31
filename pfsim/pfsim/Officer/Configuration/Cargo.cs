using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pfsim.Officer
{
    public class Cargo
    {
        public string Name { get; set; }
        public CargoType CargoType { get; set; }
        public virtual int CargoPoints { get; set; }
        public decimal Value { get; set; }
        public bool Fragile { get; set; }
        public bool Perishable { get; set; }
        public int DaysInHold { get; set; }
    }
}
