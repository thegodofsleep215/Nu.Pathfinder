using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pfsim.Officer
{
    public class Voyage
    {
        public NightStatus NightStatus { get; set; }
        public bool OpenOcean { get; set; }
        public bool ShallowWater { get; set; }
        public bool NarrowPassage { get; set; }
        public int DayOfVoyage { get; set; }
        public int DaysSinceResupply { get; set; }
        public bool VariedFoodSupplies { get; set; }
        public WeatherConditions Conditions { get; set; }

        public int GetWeatherModifier(DutyType duty)
        {
            switch (Conditions)
            {
                case WeatherConditions.Clear:
                    if (duty == DutyType.Watch)
                        return 1;
                    else
                        return 0;
                    break;
                case WeatherConditions.Fog:
                    if (duty == DutyType.Watch)
                        return -4;
                    else
                        return 0;
                    break;
                case WeatherConditions.Drizzle:
                    return -1;
                    break;
                case WeatherConditions.FairWinds:
                    if (duty == DutyType.Pilot)
                        return 2;
                    else
                        return 0;
                case WeatherConditions.Gales:
                    return -5;
                    break;
                case WeatherConditions.HeavyRain:
                    return -3;
                    break;
                case WeatherConditions.HeavySeas:
                    if (duty == DutyType.Watch)
                        return -1;
                    else
                        return -3;
                    break;
                case WeatherConditions.Hurricane:
                    return -10;
                    break;
                case WeatherConditions.Rain:
                    return -2;
                    break;
                case WeatherConditions.Storms:
                    return -3;
                    break;
                default:
                    return 0;
                    break;
            }
        }

        public int NavigationDC
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

        public int PilotingModifier
        {
            get
            {
                int dc = 0;

                dc -= ShallowWater ? 5 : 0;
                dc -= NarrowPassage ? 5 : 0;

                switch(NightStatus)
                {
                    case NightStatus.Underweigh:
                        dc -= 5;
                        break;
                    case NightStatus.Anchored:
                        dc -= 2;
                        break;
                }

                return dc;
            }
        }
    }
}
