using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  Nu.OfficerMiniGame
{
    public class SuppliesExhaustedEvent : IShipEvent
    {
        public string ShipName { get; set; }
    }
}
