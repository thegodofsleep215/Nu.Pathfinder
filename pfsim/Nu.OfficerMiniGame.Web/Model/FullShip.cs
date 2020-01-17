using Nu.OfficerMiniGame.Dal.Dto;
using System.Collections.Generic;

namespace Nu.OfficerMiniGame.Web.Model
{
    public class FullShip 
    { 
        public ShipStats Stats { get; set; }

        public ShipLoadOut Loadout { get; set; }

        public Dictionary<string, FullCrewMember> Crew { get; set; }
    }
}
