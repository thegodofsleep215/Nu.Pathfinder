using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pfsim.Officer
{
    public class Propulsion
    {
        public PropulsionType PropulsionType { get; set; }
        public int ShipSpeed { get; set;}
        public int PropulsionHitPoints { get; set; }
    }

    public enum PropulsionType
    {
        Sails,
        Oars
    }
}
