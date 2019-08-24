using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pfsim.Officer
{
    public interface IShip
    {
        string CrewName { get; }
        ShipSize ShipSize { get; }
        int TotalCrew { get; }
        int ShipDc { get; }
        int ShipPilotingBonus { get; }
        int ShipQuality { get; }
        int CommanderSkillBonus { get; }
        int ManagerSkillBonus { get; }
        int FirstWatchBonus { get; }
        List<int> WatchBonuses { get; }
        int CrewPilotModifier { get; }
        int PilotSkillBonus { get; }
        int NavigatorSkillBonus { get; }
        bool HasDisciplineOfficer { get; }
        int DisciplineSkillBonus { get; }
        int MaintainSkillBonus { get; }
        int CookSkillBonus { get; }
        int HealerSkillBonus { get; }
        bool HasHealer { get; }
        List<Assists> GetAssistance(DutyType duty);

        BaseResponse ValidateAssignedJobs();
    }
}
