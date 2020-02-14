using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace  Nu.OfficerMiniGame
{
    public class SeaShantyEvent : IShipReportEvent
    {
        DutyType _duty;

        public string ShipName { get; set; }
        public SeaShantyEvent(DutyType duty)
        {
            _duty = duty;
        }

        public override string ToString()
        {
            switch(_duty)
            {
                case DutyType.Command:
                case DutyType.Manage:
                    return "A minstril kept the crew focused on their duties.";
                case DutyType.Discipline:
                    return "A minstril helped smooth over hard feelings among the crew.";
                default:
                    return "The ministrel's shanties have cheered the crew.";
            }
        }
    }
}
