using System;
using System.Collections.Generic;
using System.Linq;

namespace Nu.OfficerMiniGame
{
    public class VoyageProgress
    {
        public string ShipName { get; set; }

        public int DayOfVoyage { get; set; }

        public int DaysSinceLastResupply { get; set; }

        public int DiseasedCrew { get; internal set; }

        public double ProgressMade { get; set; }

        public void AddDaysToVoyage(int days)
        {
            DayOfVoyage += days;
            DaysSinceLastResupply += days;
        }
    }

    public class FleetVoyageProgress
    {
        public FleetVoyageProgress() { }

        public FleetVoyageProgress(List<VoyageProgress> voyageProgresses)
        {
            DayOfVoyage = voyageProgresses.Max(x => x.DayOfVoyage);
            DaysSinceLastResupply = voyageProgresses.Max(x => x.DaysSinceLastResupply);
            ProgressMade = voyageProgresses.Max(x => x.ProgressMade);
            ShipsProgress = voyageProgresses.Select(x => new ShipsProgress
            {
                ShipName = x.ShipName,
                DiseasedCrew = x.DiseasedCrew,
            }).ToList();
        }

        public int DayOfVoyage { get; set; }

        public int DaysSinceLastResupply { get; set; }

        public double ProgressMade { get; set; }

        List<ShipsProgress> ShipsProgress { get; set; }
    }

    public class ShipsProgress
    {
        public string ShipName { get; set; }

        public int DiseasedCrew { get; internal set; }
    }
}
