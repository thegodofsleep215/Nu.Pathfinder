using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pfsim.Officer
{
    public class SiegeEngine : Cargo, ISeigeEngine
    {
        public SeigeEngineType SeigeEngineType { get; set; }
    }
}
