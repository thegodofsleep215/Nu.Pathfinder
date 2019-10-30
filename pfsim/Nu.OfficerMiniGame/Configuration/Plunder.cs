using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  Nu.OfficerMiniGame
{
    public class Plunder : Cargo, IPlunder
    {
        public PlunderCategory PlunderCategory { get; set; }
        public bool IsLivestock { get; set; } = false;
    }
}
