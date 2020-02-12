using Nu.OfficerMiniGame.Dal.Dto;
using Nu.OfficerMiniGame.Dal.Enums;
using System;
using System.Collections.Generic;

namespace Nu.OfficerMiniGame
{
    public enum WatchShift
    {
        First,
        Second,
        Third
    }

    public class MiniGameStatus
    {
        public MiniGameStatus(WeatherConditions weatherConditions)
        {
            WeatherConditions = weatherConditions;
        }

        public WeatherConditions WeatherConditions { get; set; }

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

        public int CommandResult { get; set; }

        public int CommandModifier
        {
            get
            {
                if (CommandResult >= 5) return 2;
                if (CommandResult >= 0) return 0;
                return Math.Abs(CommandResult) / 5 * -2;
            }
        }

        public int ManageResult { get; set; }

        public int ManageModifier
        {
            get
            {
                if (ManageResult <= -10) return -4;
                return 0;
            }
        }

        public List<int> WatchResults { get; set; } = new List<int>();

        public List<int> MinistrelResults { get; set; } = new List<int>();

        public int WatchModifier
        {
            get
            {
                int retval = 0;
                foreach (var result in WatchResults)
                {
                    retval += result < 0 ? -2 : 0;
                }

                return retval;
            }
        }

        public int PilotResult { get; set; }

        public int NavigationResult { get; set; }

        public int MaintainResult { get; set; }

        public int CookResult { get; internal set; }

        public List<object> GameEvents { get; }
    }

    public class SailingParameters
    {
        public string VoyageName { get; set; }

        public bool NarrowPassage { get; set; }

        public bool ShallowWater { get; set; }

        public bool OpenOcean { get; set; }

        public NightStatus NightStatus { get; set; }

        public List<ShipModifiers> ShipModifiers { get; set; }

        public int PilotingModifier
        {
            get
            {
                int dc = 0;

                dc -= ShallowWater ? 5 : 0;
                dc -= NarrowPassage ? 5 : 0;

                switch (NightStatus)
                {
                    case NightStatus.Underweigh:
                        dc -= 5;
                        break;
                    case NightStatus.Drifting:
                        dc -= 2;
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

    }

    public class ShipModifiers
    {
        public string LoadoutName { get; set; }
        public int MoraleModifier { get; set; }

        public int DisciplineModifier { get; set; }

        public int CommandModifier { get; set; }

        public int NumberOfCrewUnfitForDuty { get; set; }

        public int NumberOfCrewDiseased { get; set; }
        public DisciplineStandards DisciplineStandards { get; set; }

        public int Swabbies { get; set; }
    }


}
