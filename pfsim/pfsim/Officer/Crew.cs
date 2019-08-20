namespace pfsim.Officer
{
    public class Crew
    {

        public ShipSize ShipSize { get; set; }

        public int ShipDc { get; set; }

        public int CommanderSkillBonus { get; set; }

        public int ManagerSkillBonus { get; set; }

        public int FirstWatchBonus { get; set; }

        public int CrewSize { get; set; }
        public int CrewPilotModifier { get; internal set; }
        public int PilotSkillBonus { get; internal set; }
        public int NavigatorSkillBonus { get; internal set; }
        public bool HasDisciplineOfficer { get; internal set; }
        public int DisciplineSkillBonus { get; internal set; }
        public int MaintainSkillBonus { get; internal set; }
    }
}
