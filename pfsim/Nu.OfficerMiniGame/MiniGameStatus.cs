using System;
using System.Collections.Generic;

namespace Nu.OfficerMiniGame
{
    public class MiniGameStatus
    {
        public MiniGameStatus(bool openOcean, NightStatus nightStatus)
        {
            GameEvents = new List<object> {
                new DawnOfANewDayEvent
                {
                    OpenOcean = openOcean,
                    NightStatus = nightStatus
                }
            };
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

        public WeatherConditions WeatherConditions { get; set; }

        public int GetWeatherModifier(DutyType duty)
        {
            switch (WeatherConditions)
            {
                case WeatherConditions.Clear:
                    if (duty == DutyType.Watch)
                        return 1;
                    else
                        return 0;
                case WeatherConditions.Fog:
                    if (duty == DutyType.Watch)
                        return -4;
                    else
                        return 0;
                case WeatherConditions.Drizzle:
                    return -1;
                case WeatherConditions.FairWinds:
                    if (duty == DutyType.Pilot)
                        return 2;
                    else
                        return 0;
                case WeatherConditions.Gales:
                    return -5;
                case WeatherConditions.HeavyRain:
                    return -3;
                case WeatherConditions.HeavySeas:
                    if (duty == DutyType.Watch)
                        return -1;
                    else
                        return -3;
                case WeatherConditions.Hurricane:
                    return -10;
                case WeatherConditions.Rain:
                    return -2;
                case WeatherConditions.Storms:
                    return -3;
                default:
                    return 0;
            }
        }



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
                int dc = 12 + GetWeatherModifier(DutyType.Navigate);

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
