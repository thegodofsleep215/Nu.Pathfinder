using Nu.OfficerMiniGame.Dal.Dto;
using System.Collections.Generic;
using System.Linq;

namespace Nu.OfficerMiniGame
{
    public class FleetVoyageProgress
    {
        public FleetVoyageProgress() { }

        public FleetVoyageProgress(List<VoyageProgress> voyageProgresses, WeatherConditions weatherConditions)
        {
            DayOfVoyage = voyageProgresses.Max(x => x.DayOfVoyage);
            DaysSinceLastResupply = voyageProgresses.Max(x => x.DaysSinceLastResupply);
            ProgressMade = voyageProgresses.Max(x => x.ProgressMade);
            WeatherConditions = weatherConditions;
            ShipsProgress = voyageProgresses.Select(x => new ShipsProgress
            {
                ShipName = x.ShipName,
                DiseasedCrew = x.DiseasedCrew,
            }).ToList();
        }

        public int DayOfVoyage { get; set; }

        public int DaysSinceLastResupply { get; set; }

        public double ProgressMade { get; set; }

        public WeatherConditions WeatherConditions { get; set; }

        public List<ShipsProgress> ShipsProgress { get; set; }
    }
}
