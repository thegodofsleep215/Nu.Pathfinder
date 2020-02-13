using Nu.OfficerMiniGame.Dal.Dto;

namespace Nu.OfficerMiniGame.Web.Model
{
    public class VoyageStatus
    {
        public PfDateTime CurrentDate { get; set; }

        public int DaysPlanned { get; set; }

        public double TravelProgress { get; set; }
    }
}
