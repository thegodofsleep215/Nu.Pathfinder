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

        public virtual decimal SellableValue
        {
            get
            {
                return Value;
            }
        }

        public override string ToString()
        {
            return string.Format("{0} - {1} worth {2} gp and filling {3} cargo points.", CargoType.ToString(), Name, SellableValue, CargoPoints);
        }
    }
}
