using Nu.OfficerMiniGame.Dal.Dto;
using Nu.OfficerMiniGame.Dal.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Nu.OfficerMiniGame
{
    public class FleetState
    {
        public FleetState() { }

        public PfDateTime StartDate { get; set; }

        public PfDateTime CurrentDate { get; set; }

        public int DayOfVoyage { get; set; }

        public int DaysSinceLastResupply { get; set; }

        public double ProgressMade { get; set; }

        public int DaysPlanned { get; set; }

        public bool OpenOcean { get; set; }

        public bool ShallowWater { get; set; }

        public bool NarrowPassage { get; set; }

        public NightStatus NightStatus { get; set; }

        public WeatherConditions WeatherConditions { get; set; }
     
        public int PilotingModifier
        {
            get
            {
                int dc = 0;

                dc += ShallowWater ? 5 : 0;
                dc += NarrowPassage ? 5 : 0;

                switch (NightStatus)
                {
                    case NightStatus.Underweigh:
                        dc += 5;
                        break;
                    case NightStatus.Drifting:
                        dc += 2;
                        break;
                }

                return dc;
            }
        }

        public int NavigationModifier
        {
            get
            {
                int dc = 12;

                dc += OpenOcean ? 5 : 0;

                if (NightStatus == NightStatus.Underweigh)
                    dc += 5;

                return dc;
            }
        }


        public Dictionary<string, ShipState> ShipStates { get; set; } = new Dictionary<string, ShipState>();
        
        public int GetWeatherModifier(DutyType duty)
        {
            if (WeatherConditions == null)
            {
                return 0;
            }
            if (duty == DutyType.Watch) throw new ArgumentException("Use GetWatchModifier");
            var temp = 0;

            if (duty != DutyType.Command && duty != DutyType.Cook && duty != DutyType.Discipline && duty != DutyType.Heal)
            {
                switch (WeatherConditions.PrecipitationType)
                {
                    case PrecipitationType.Sleet:
                    case PrecipitationType.LightSnow:
                    case PrecipitationType.Drizzle:
                        temp += 1;
                        break;
                    case PrecipitationType.MediumSnow:
                    case PrecipitationType.Rain:
                        temp += 2;
                        break;
                    case PrecipitationType.HeavySnow:
                    case PrecipitationType.HeavyRain:
                    case PrecipitationType.Sandstorm:
                        temp += 3;
                        break;
                    case PrecipitationType.Blizzard:
                    case PrecipitationType.Hail:
                    case PrecipitationType.Thundersnow:
                    case PrecipitationType.Thunderstorm:
                        temp += 4;
                        break;
                    case PrecipitationType.Hurricane:
                    case PrecipitationType.Tornado:
                        temp += 5;
                        break;

                }
            }

            switch (WeatherConditions.WindSpeed)
            {
                case WindSpeed.Light:
                    if (duty == DutyType.Pilot)
                    {
                        temp -= 2;
                    }
                    break;
                case WindSpeed.Strong:
                    temp += 2;
                    break;
                case WindSpeed.Severe:
                    temp += 4;
                    break;
                case WindSpeed.Windstorm:
                    temp += 8;
                    break;

            }



            return temp;
        }

        public int GetWatchModifier(WatchShift watchNumber)
        {
            if (WeatherConditions == null)
            {
                return 0;
            }
            var temp = 0;

            switch (WeatherConditions.PrecipitationType)
            {
                case PrecipitationType.None:
                    temp -= 1;
                    break;
                case PrecipitationType.HeavyFog:
                    if (watchNumber == WatchShift.First)
                        temp += 20;
                    break;
                case PrecipitationType.MediumFog:
                    if (watchNumber == WatchShift.First)
                        temp += 4;
                    break;
                case PrecipitationType.LightFog:
                    if (watchNumber == WatchShift.First)
                        temp += 2;
                    break;
                case PrecipitationType.Sleet:
                case PrecipitationType.LightSnow:
                case PrecipitationType.Drizzle:
                    temp += 2;
                    break;
                case PrecipitationType.MediumSnow:
                case PrecipitationType.Rain:
                    temp += 4;
                    break;
                case PrecipitationType.HeavySnow:
                case PrecipitationType.HeavyRain:
                case PrecipitationType.Sandstorm:
                    temp += 6;
                    break;
                case PrecipitationType.Blizzard:
                case PrecipitationType.Hail:
                case PrecipitationType.Thundersnow:
                case PrecipitationType.Thunderstorm:
                    temp += 8;
                    break;
                case PrecipitationType.Hurricane:
                case PrecipitationType.Tornado:
                    temp += 20;
                    break;

            }

            switch (WeatherConditions.WindSpeed)
            {
                case WindSpeed.Strong:
                    temp += 2;
                    break;
                case WindSpeed.Severe:
                    temp += 4;
                    break;
                case WindSpeed.Windstorm:
                    temp += 8;
                    break;

            }

            return temp;
        }

        public void ResetProgress()
        {
            DayOfVoyage = 0;
            ProgressMade = 0;
        }

        public void AddDaysToVoyage(int days)
        {
            DayOfVoyage += days;
            DaysSinceLastResupply += days;
            CurrentDate = StartDate + TimeSpan.FromDays(DayOfVoyage);
        }

    }
}
