using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  Nu.OfficerMiniGame
{
    public class SiegeEngine : Cargo, ISeigeEngine
    {
        public SiegeEngineType SeigeEngineType { get; set; }
    }
}
