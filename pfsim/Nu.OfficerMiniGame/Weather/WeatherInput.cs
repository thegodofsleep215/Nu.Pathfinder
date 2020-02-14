using Nu.OfficerMiniGame.Dal.Dto;
using System;

namespace Nu.OfficerMiniGame.Weather
{
    public class WeatherInput
    {
        public WeatherInput(WeatherConditions lastConditions, int elevationFt, PfDateTime date, Region region)
        {
            LastConditions = lastConditions;
            ElevationFt = elevationFt;
            Date = date;
            Region = region;
        }

        public WeatherConditions LastConditions { get; }

        public int ElevationFt { get; }

        public Region Region { get; }

        public PfDateTime Date { get; }


        public Season CurrentSeason { get { return Date.Season; } }

        public int DaysLeftOfTemperatue
        {
            get
            {
                int d = 0;
                if (LastConditions != null)
                {
                    d = LastConditions.DurationOfTemperature - 1;
                }
                return d < 0 ? 0 : d;

            }
        }

        public PrecipitationFrequency PrecipitationFrequency
        {
            get
            {
                switch (Region)
                {
                    case Region.Temperate:
                    case Region.Cold:
                        switch (CurrentSeason)
                        {
                            case Season.Winter:
                                return PrecipitationFrequency.Rare;
                            case Season.Spring:
                                return PrecipitationFrequency.Intermittent;
                            case Season.Summer:
                                return PrecipitationFrequency.Common;
                            case Season.Fall:
                                return PrecipitationFrequency.Intermittent;
                        }
                        break;
                    case Region.Tropical:
                        switch (CurrentSeason)
                        {
                            case Season.Winter:
                                return PrecipitationFrequency.Rare;
                            case Season.Spring:
                                return PrecipitationFrequency.Common;
                            case Season.Summer:
                                return PrecipitationFrequency.Intermittent;
                            case Season.Fall:
                                return PrecipitationFrequency.Common;
                        }
                        break;
                }
                throw new NotImplementedException();
            }
        }

        public PrecipitationIntensity PrecipitationIntensity
        {
            get
            {
                if (ElevationFt < 1000)
                {
                    if (Region == Region.Cold)
                    {
                        return PrecipitationIntensity.Medium;
                    }
                    return PrecipitationIntensity.Heavy;
                }
                else if (ElevationFt < 5000)
                {
                    if (Region == Region.Cold)
                    {
                        return PrecipitationIntensity.Light;
                    }
                    else if (Region == Region.Tropical)
                    {
                        return PrecipitationIntensity.Heavy;
                    }
                    return PrecipitationIntensity.Medium;
                }
                else
                {
                    if (Region == Region.Tropical)
                    {
                        return PrecipitationIntensity.Medium;
                    }
                    return PrecipitationIntensity.Light;
                }
            }
        }
    }
}
