using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pfsim.Officer
{
    public interface IShip
    {
        string CrewName { get; set; }
        ShipSize ShipSize { get; set; }
        int CrewSize { get; set; }
        int ShipDc { get; set; }
        int ShipPilotingBonus { get; set; }
        int ShipQuality { get; set; }
        int CommanderSkillBonus { get; set; }
        int ManagerSkillBonus { get; set; }
        int FirstWatchBonus { get; set; }
        int CrewPilotModifier { get; set; }
        int PilotSkillBonus { get; set; }
        int NavigatorSkillBonus { get; set; }
        bool HasDisciplineOfficer { get; set; }
        int DisciplineSkillBonus { get; set; }
        int MaintainSkillBonus { get; set; }
    }
}
