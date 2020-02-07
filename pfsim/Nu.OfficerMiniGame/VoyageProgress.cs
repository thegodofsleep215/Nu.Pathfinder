using Nu.OfficerMiniGame.Dal.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nu.OfficerMiniGame
{
    public class VoyageProgress
    {
        public string ShipName { get; set; }

        public PfDateTime StartDate { get; set; }

        public PfDateTime CurrentDate { get; set; }

        public int DayOfVoyage { get; set; }

        public int DaysSinceLastResupply { get; set; }

        public int DiseasedCrew { get; internal set; }

        public double ProgressMade { get; set; }

        public void AddDaysToVoyage(int days)
        {
            DayOfVoyage += days;
            DaysSinceLastResupply += days;
            CurrentDate = StartDate + TimeSpan.FromDays(DayOfVoyage);
        }

        public void ResetProgress()
        {
            DayOfVoyage = 0;
            ProgressMade = 0;
        }
    }
}
