using System.Collections.Generic;

namespace pfsim.Officer
{
    public class Crew : IShip
    {
        public string CrewName { get; set; }

        public ShipSize ShipSize { get; set; }

        public int ShipDc { get; set; }

        public int CommanderSkillBonus { get; set; }

        public int ManagerSkillBonus { get; set; }

        public int FirstWatchBonus { get; set; }

        public List<int> WatchBonuses { get; set; }

        public int ShipPilotingBonus { get; set; }

        public int ShipQuality { get; set; }

        public int TotalCrew { get; set; }

        public int CrewPilotModifier { get; set; }

        public int PilotSkillBonus { get; set; }

        public int NavigatorSkillBonus { get; set; }

        public bool HasDisciplineOfficer { get; set; }

        public int DisciplineSkillBonus { get; set; }

        public int MaintainSkillBonus { get; set; }

        public int CookSkillBonus { get; set; }

        public bool HasHealer { get; set; }

        public int HealerSkillBonus { get; set; }

        // The simple ship can't track assistants.
        public List<Assists> GetAssistance(DutyType duty)
        {
            List<Assists> retval = new List<Assists>();

            return retval;
        }

        public BaseResponse ValidateAssignedJobs()
        {
            BaseResponse retval = new BaseResponse();

            return retval;
        }
    }
}
