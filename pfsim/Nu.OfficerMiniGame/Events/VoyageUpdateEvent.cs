using Nu.OfficerMiniGame.Dal.Dto;

namespace Nu.OfficerMiniGame
{
    public class VoyageUpdateEvent
    {
        public PfDateTime StartDate { get; set; }

        public int DaysPlanned { get; set; }

        public string[] ShipLoadouts { get; set; }
    }
}
