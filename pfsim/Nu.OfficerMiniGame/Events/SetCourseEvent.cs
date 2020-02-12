using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Nu.OfficerMiniGame.Dal.Dto
{
    public class SetCourseEvent
    {
        public PfDateTime StartDate { get; set; }

        public string Port { get; set; }

        public string DestinationPort { get; set; }

        public string[] ShipLoadouts { get; set; }

        public int DaysPlanned { get; set; }

        public NightStatus NightStatus { get; set; }

        public bool OpenOcean { get; set; }

        public bool ShallowWater { get; set; }

        public bool NarrowPassage { get; set; }

        public DisciplineStandards DisciplineStandards { get; set; }

        public List<SwabbieCount> Swabbies { get; set; }
    }
}
