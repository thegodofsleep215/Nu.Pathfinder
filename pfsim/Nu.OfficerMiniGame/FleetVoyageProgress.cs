using Nu.OfficerMiniGame.Dal.Dto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nu.OfficerMiniGame
{
    public class FleetVoyageProgress
    {
        public FleetVoyageProgress() { }

        public PfDateTime StartDate { get; set; }

        public PfDateTime CurrentDate { get; set; }

        public int DayOfVoyage { get; set; }

        public int DaysSinceLastResupply { get; set; }

        public double ProgressMade { get; set; }

        public int DaysPlanned { get; set; }

        public WeatherConditions WeatherConditions { get; set; }

        public Dictionary<string, ShipState> ShipStates { get; set; } = new Dictionary<string, ShipState>();

        public Dictionary<string, double> ProgressForEachDay { get; set; } = new Dictionary<string, double>();

        public bool OpenOcean { get; set; }

        public void ResetProgress()
        {
            DayOfVoyage = 0;
            ProgressMade = 0;
            ProgressForEachDay.Clear();
        }

        public void AddDaysToVoyage(int days)
        {
            DayOfVoyage += days;
            DaysSinceLastResupply += days;
            CurrentDate = StartDate + TimeSpan.FromDays(DayOfVoyage);
        }

    }
}
