using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pfsim.Officer.Configuration
{
    public class Plunder : Cargo, IPlunder
    {
        public PlunderCategory PlunderCategory { get; set; }
    }
}
