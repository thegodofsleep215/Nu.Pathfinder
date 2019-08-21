﻿namespace pfsim.Officer
{
    public class Crew
    {
        public string CrewName { get; set; }

        public ShipSize ShipSize { get; set; }

        public int ShipDc { get; set; }

        public int CommanderSkillBonus { get; set; }

        public int ManagerSkillBonus { get; set; }

        public int FirstWatchBonus { get; set; }

        public int CrewSize { get; set; }
        public int CrewPilotModifier { get; set; }
        public int PilotSkillBonus { get; set; }
        public int NavigatorSkillBonus { get; set; }
        public bool HasDisciplineOfficer { get; set; }
        public int DisciplineSkillBonus { get; set; }
        public int MaintainSkillBonus { get; set; }
        public int CookSkillBonus { get; set; }
        public bool HasHealer { get; set; }
        public int HealerBonus { get; set; }
    }
}
