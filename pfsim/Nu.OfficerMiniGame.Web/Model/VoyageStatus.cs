using Nu.OfficerMiniGame.Dal.Dto;
using System.Collections.Generic;
using System.Linq;

namespace Nu.OfficerMiniGame.Web.Model
{
    public class VoyageStatus
    {
        public PfDateTime CurrentDate { get; set; }

        public int DaysPlanned { get; set; }

        public double ProgressMade { get; set; }

        public WeatherConditions WeatherConditions { get; set; }

        public List<ShipsProgress> ShipsProgress { get; set; }

        public static VoyageStatus FromFleetState(FleetState state)
        {
            var vs = new VoyageStatus();
            vs.CurrentDate = state.CurrentDate;
            vs.DaysPlanned = state.DaysPlanned;
            vs.ProgressMade = state.ProgressMade;
            vs.WeatherConditions = state.WeatherConditions;
            vs.ShipsProgress = state.ShipStates.Select(x => new ShipsProgress
            {
                ShipName = x.Key,
                DiseasedCrew = x.Value.DiseasedCrew
            }).ToList();
            return vs;
        }
    }
}
