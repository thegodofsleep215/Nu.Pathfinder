using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nu.OfficerMiniGame
{
    public class PerformedDutyEvent
    {
        public DutyType Duty { get; set; }

        public string CrewMember { get; set; }

        public int DC { get; set; }

        public int Result { get; set; }

        public int Assist { get; set; }

        public int SkillBonus { get; set; }

        public string ShipName { get; set; }

        public PerformedDutyEvent() { }

        public PerformedDutyEvent(string shipName, DutyType duty, string crew, int dc, int assist, int skill, int result)
        {
            ShipName = shipName;
            Duty = duty;
            CrewMember = crew;
            DC = dc;
            Assist = assist;
            SkillBonus = skill;
            Result = result;
        }

        public override string ToString()
        {
            return $"{CrewMember} performed the duty of {Duty} which had a difficulty of {DC}.  The sailor had a skill of {SkillBonus} with an assistance of {Assist}, and the result was {Result}.";
        }
    }
}
