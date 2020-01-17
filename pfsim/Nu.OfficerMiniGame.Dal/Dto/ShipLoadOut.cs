using Nu.OfficerMiniGame.Dal.Enums;
using System.Collections.Generic;

namespace Nu.OfficerMiniGame.Dal.Dto
{
    public class ShipLoadOut
    {
        public string Name { get; set; }

        public string ShipName { get; set; }

        public List<LoadoutCrewMember> CrewMembers { get; set; }
    }
}
