﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  Nu.OfficerMiniGame.Events
{
    public class RepairFailureEvent : IShipReportEvent
    {
        public string ShipName { get; set; }
    }
}
