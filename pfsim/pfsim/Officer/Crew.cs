using System.Collections.Generic;

namespace pfsim.Officer
{
    public class Crew : IShip
    {
        public string CrewName { get; set; }
        public ShipSize ShipSize { get; set; }
        public int CrewSize { get; set; }
        public int ShipDc { get; set; }
        public int ShipPilotingBonus { get; set; }
        public int ShipQuality { get; set; }
        public int CommanderSkillBonus { get; set; }
        public int ManagerSkillBonus { get; set; }
        public int FirstWatchBonus { get; set; }      
        public int CrewPilotModifier { get; set; }
        public int PilotSkillBonus { get; set; }
        public int NavigatorSkillBonus { get; set; }
        public bool HasDisciplineOfficer { get; set; }
        public int DisciplineSkillBonus { get; set; }
        public int MaintainSkillBonus { get; set; }

        public List<Assists> GetAssistance(DutyType duty)
        {
            List<Assists> retval = new List<Assists>();

            return retval;
        }
    }
}
