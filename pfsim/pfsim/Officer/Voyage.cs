using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pfsim.Officer
{
    public class Voyage
    {
        public NightStatus NightStatus { get; set; }
        public bool OpenOcean { get; set; }
        public bool ShallowWater { get; set; }
        public bool NarrowPassage { get; set; }
        public int DayOfVoyage { get; set; }
        public int DaysSinceResupply { get; set; }
        public int VariedFoodSupplies { get; set; }
        public Weather Weather { get; set; }
    }
}
