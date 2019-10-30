using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nu.OfficerMiniGame
{
    public class Voyage
    {
        public NightStatus NightStatus { get; set; }
        public bool OpenOcean { get; set; }  
        public bool ShallowWater { get; set; } 
        public bool NarrowPassage { get; set; }  
        public int DayOfVoyage { get; set; }
        public int DaysSinceResupply { get; set; }
        public int HullDamageSinceRefit { get; set; }
        public int CurrentHullDamage { get; set; }
        public int SailDamageSinceRefit { get; set; }
        public int CurrentSailDamage { get; set; }
        public bool VariedFoodSupplies { get; set; }
        [JsonIgnore]
        public bool DiseaseAboardShip
        {
            get
            {
                return DiseasedCrew > 0;
            }
        }
        public int DiseasedCrew { get; set; }
        public int CrewUnfitForDuty { get; set; }
        // Positive increases command problems.  Negative reduces them.
        public int CommandModifier { get; set; }
        // Positive increases discipline problems.  Negative reduces them.
        public int DisciplineModifier { get; set; }
        public int NumberOfCrewPlottingMutiny { get; set; }
        public WeatherConditions WeatherConditions { get; set; }

        public void AlterHullDamage(int damage)
        {
            if(damage > 0)
            {
                CurrentHullDamage += damage;
                HullDamageSinceRefit += damage;
            }
            else
            {
                if ((CurrentHullDamage - Math.Ceiling(HullDamageSinceRefit * .1)) >= Math.Abs(damage))
                    CurrentHullDamage += damage;
                else
                    CurrentHullDamage = Convert.ToInt32((Math.Ceiling(HullDamageSinceRefit * .1)));
            }
        }

        public void AlterSailDamage(int damage)
        {
            if (damage > 0)
            {
                CurrentSailDamage += damage;
                SailDamageSinceRefit += damage;
            }
            else
            {
                if ((CurrentSailDamage - Math.Ceiling(SailDamageSinceRefit * .1)) >= Math.Abs(damage))
                    CurrentSailDamage += damage;
                else
                    CurrentSailDamage = Convert.ToInt32((Math.Ceiling(SailDamageSinceRefit * .1)));
            }
        }

        internal void AddDaysToVoyage(int days)
        {
            DaysSinceResupply += days;
            DayOfVoyage += days;
        }

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

        [JsonIgnore]
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

        [JsonIgnore]
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
                    case NightStatus.Drifting:
                        dc -= 2;
                        break;
                }

                return dc;
            }
        }
    }
}
