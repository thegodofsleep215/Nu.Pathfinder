using Nu.OfficerMiniGame.Dal.Dto;
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
}
