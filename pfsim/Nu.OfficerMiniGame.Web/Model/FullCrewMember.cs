using Nu.OfficerMiniGame.Dal.Dto;

namespace Nu.OfficerMiniGame.Web.Model
{
    public class FullCrewMember
    {
        public CrewMemberStats Stats { get; set; }

        public LoadoutCrewMember Loadout { get; set; }
    }
}
