using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Nu.Game.Common;
using System;

namespace Nu.OfficerMiniGame
{
    public interface IWeatherEngine
    {
        WeatherConditions GetWeatherConditions(WeatherInput input);
    }

    public class WeatherConditions
    {
        public WeatherStatus WeatherStatus { get; set; }

        public int TemperatureInF { get; set; }

        public int DurationOfTemperature { get; set; }
        public PrecipitationType PrecipitationType { get; internal set; }
        public int PreciptationStartTime { get; internal set; }
        public int PrecipitationHours { get; internal set; }
    }

    public class WeatherInput
    {
        public WeatherConditions LastConditions { get; set; }

        public int ElevationFt { get; set; }

        public Season CurrentSeason { get; set; }

        public Region Region { get; set; }

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

    [JsonConverter(typeof(StringEnumConverter))]
    public enum Season
    {
        Spring,
        Summer,
        Fall,
        Winter
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum Region
    {
        Cold,
        Temperate,
        Tropical,
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum PrecipitationFrequency
    {
        Drought,
        Rare,
        Intermittent,
        Common,
        Constant
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum PrecipitationIntensity
    {
        Light,
        Medium,
        Heavy,
        Torrential
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum PrecipitationType
    {
        None,

        LightFog,
        MediumFog,
        HeavyFog,

        Drizzle,
        LightRain,
        Rain,
        HeavyRain,
        Thunderstorm,

        Sleet,

        LightSnow,
        MediumSnow,
        HeavySnow,
        Blizard
    }

    public class PathfinderWeatherGenerator : IWeatherEngine
    {
        public WeatherConditions GetWeatherConditions(WeatherInput input)
        {
            var cond = new WeatherConditions();
            if (input.DaysLeftOfTemperatue == 0)
            {
                cond.TemperatureInF = InitialTemp(input.Region, input.CurrentSeason);
                cond.TemperatureInF += TempeatureAdjustmentForElevation(input.ElevationFt);
                var adjustment = AdjustTemperatue(input.Region);
                cond.TemperatureInF += adjustment.tempAdjust;
                cond.DurationOfTemperature += adjustment.days;
            }
            else
            {
                cond.TemperatureInF = input.LastConditions.TemperatureInF;
                cond.DurationOfTemperature = input.LastConditions.DurationOfTemperature - 1;
            }

            var hasPrecipitation = HasPrecipitation(input.PrecipitationFrequency);
            if (hasPrecipitation.hasPrecipitation)
            {
                var precipitation = GetPrecipitationType(input.PrecipitationIntensity, cond.TemperatureInF);
                cond.PrecipitationType = precipitation.type;
                cond.PrecipitationHours = precipitation.hours;
                cond.PreciptationStartTime = hasPrecipitation.timeOfDay;
            }
            else
            {
                cond.PrecipitationType = PrecipitationType.None;
            }

            // TODO: Wind

            // TODO: Cloud Cover

            // TODO: Severe Weather Events

            return cond;
        }

        private int InitialTemp(Region region, Season season)
        {
            switch (region)
            {
                case Region.Cold:
                    switch (season)
                    {
                        case Season.Winter:
                            return 20;
                        case Season.Spring:
                            return 30;
                        case Season.Summer:
                            return 40;
                        case Season.Fall:
                            return 30;
                    }
                    break;
                case Region.Temperate:
                    switch (season)
                    {
                        case Season.Winter:
                            return 30;
                        case Season.Spring:
                            return 60;
                        case Season.Summer:
                            return 40;
                        case Season.Fall:
                            return 30;
                    }
                    break;
                case Region.Tropical:
                    switch (season)
                    {
                        case Season.Winter:
                            return 50;
                        case Season.Spring:
                            return 75;
                        case Season.Summer:
                            return 95;
                        case Season.Fall:
                            return 75;
                    }
                    break;
            }
            throw new NotImplementedException(); // Should never get here.
        }

        private (int tempAdjust, int days) AdjustTemperatue(Region region)
        {
            var percent = DiceRoller.DPercentile();
            switch (region)
            {
                case Region.Cold:
                    if (percent <= 20)
                    {
                        return (DiceRoller.D10(3) * -1, DiceRoller.D4(1));
                    }
                    else if (percent <= 40)
                    {
                        return (DiceRoller.D10(2) * -1, DiceRoller.D6(1) + 1);
                    }
                    else if (percent <= 60)
                    {
                        return (DiceRoller.D10(1) * -1, DiceRoller.D6(1) + 2);
                    }
                    else if (percent <= 80)
                    {
                        return (0, DiceRoller.D6(1) + 2);
                    }
                    else if (percent <= 95)
                    {
                        return (DiceRoller.D10(1), DiceRoller.D6(1) + 1);
                    }
                    else if (percent <= 99)
                    {
                        return (DiceRoller.D10(2), DiceRoller.D4(1));
                    }
                    else
                    {
                        return (DiceRoller.D10(3), DiceRoller.D2(1));
                    }
                case Region.Temperate:
                    if (percent <= 5)
                    {
                        return (DiceRoller.D10(3) * -1, DiceRoller.D2(1));
                    }
                    else if (percent <= 15)
                    {
                        return (DiceRoller.D10(2) * -1, DiceRoller.D4(1));
                    }
                    else if (percent <= 35)
                    {
                        return (DiceRoller.D10(1) * -1, DiceRoller.D4(1) + 1);
                    }
                    else if (percent <= 65)
                    {
                        return (0, DiceRoller.D6(1) + 1);
                    }
                    else if (percent <= 85)
                    {
                        return (DiceRoller.D10(1), DiceRoller.D4(1) + 1);
                    }
                    else if (percent <= 95)
                    {
                        return (DiceRoller.D10(2), DiceRoller.D4(1));
                    }
                    else
                    {
                        return (DiceRoller.D10(3), DiceRoller.D2(1));
                    }
                case Region.Tropical:
                    if (percent <= 10)
                    {
                        return (DiceRoller.D10(2) * -1, DiceRoller.D2(1));
                    }
                    else if (percent <= 25)
                    {
                        return (DiceRoller.D10(1) * -1, DiceRoller.D2(1));
                    }
                    else if (percent <= 55)
                    {
                        return (0, DiceRoller.D4(1));
                    }
                    else if (percent <= 85)
                    {
                        return (DiceRoller.D10(1), DiceRoller.D4(1));
                    }
                    else
                    {
                        return (DiceRoller.D10(2), DiceRoller.D2(1));
                    }
            }
            throw new NotImplementedException();

        }

        private int TempeatureAdjustmentForElevation(int elevation)
        {
            if (elevation < 1000)
            {
                return 10;
            }
            else if (elevation < 5000)
            {
                return 0;
            }
            return -10;
        }

        private (bool hasPrecipitation, int timeOfDay) HasPrecipitation(PrecipitationFrequency pf)
        {
            var chance = DiceRoller.DPercentile();
            switch (pf)
            {
                case PrecipitationFrequency.Drought:
                    if (chance <= 5)
                    {
                        return (true, DiceRoller.D24(1));
                    }
                    return (false, 0);
                case PrecipitationFrequency.Rare:
                    if (chance <= 15)
                    {
                        return (true, DiceRoller.D24(1));
                    }
                    return (false, 0);

                case PrecipitationFrequency.Intermittent:
                    if (chance <= 30)
                    {
                        return (true, DiceRoller.D24(1));
                    }
                    return (false, 0);

                case PrecipitationFrequency.Common:
                    if (chance <= 60)
                    {
                        return (true, DiceRoller.D24(1));
                    }
                    return (false, 0);


                case PrecipitationFrequency.Constant:
                    if (chance <= 95)
                    {
                        return (true, DiceRoller.D24(1));
                    }
                    return (false, 0);


            }
            throw new NotImplementedException();
        }

        private (PrecipitationType type, int hours) GetPrecipitationType(PrecipitationIntensity pi, int temperature)
        {
            var percent = DiceRoller.DPercentile();
            switch (pi)
            {
                case PrecipitationIntensity.Light:
                    if (temperature > 32)
                    {
                        if (percent <= 20)
                        {
                            return (PrecipitationType.LightFog, DiceRoller.D8(1));
                        }
                        else if (percent <= 40)
                        {
                            return (PrecipitationType.MediumFog, DiceRoller.D6(1));
                        }
                        else if (percent <= 50)
                        {
                            return (PrecipitationType.Drizzle, DiceRoller.D4(1));
                        }
                        else if (percent <= 75)
                        {
                            return (PrecipitationType.Drizzle, DiceRoller.D12(2));
                        }
                        else if (percent <= 90)
                        {
                            return (PrecipitationType.LightRain, DiceRoller.D4(1));
                        }
                        return (temperature <= 40 ? PrecipitationType.Sleet : PrecipitationType.LightRain, DiceRoller.D4(1));
                    }
                    else
                    {

                        if (percent <= 20)
                        {
                            return (PrecipitationType.LightFog, DiceRoller.D6(1));
                        }
                        else if (percent <= 40)
                        {
                            return (PrecipitationType.LightFog, DiceRoller.D8(1));
                        }
                        else if (percent <= 50)
                        {
                            return (PrecipitationType.MediumFog, DiceRoller.D4(1));
                        }
                        else if (percent <= 60)
                        {
                            return (PrecipitationType.LightSnow, 1);
                        }
                        else if (percent <= 75)
                        {
                            return (PrecipitationType.LightSnow, DiceRoller.D4(1));
                        }
                        return (PrecipitationType.LightSnow, DiceRoller.D12(2));
                    }
                case PrecipitationIntensity.Medium:
                    if (temperature > 32)
                    {
                        if (percent <= 10)
                        {
                            return (PrecipitationType.MediumFog, DiceRoller.D6(1));
                        }
                        else if (percent <= 20)
                        {
                            return (PrecipitationType.MediumFog, DiceRoller.D12(1));
                        }
                        else if (percent <= 30)
                        {
                            return (PrecipitationType.HeavyFog, DiceRoller.D4(1));
                        }
                        else if (percent <= 35)
                        {
                            return (PrecipitationType.Rain, DiceRoller.D4(1));
                        }
                        else if (percent <= 70)
                        {
                            return (PrecipitationType.Rain, DiceRoller.D8(1));
                        }
                        else if (percent <= 90)
                        {
                            return (PrecipitationType.Rain, DiceRoller.D12(2));
                        }
                        return (temperature <= 40 ? PrecipitationType.Sleet : PrecipitationType.Rain, DiceRoller.D4(1));
                    }
                    else
                    {
                        if (percent <= 10)
                        {
                            return (PrecipitationType.MediumFog, DiceRoller.D6(1));
                        }
                        else if (percent <= 20)
                        {
                            return (PrecipitationType.MediumFog, DiceRoller.D8(1));
                        }
                        else if (percent <= 30)
                        {
                            return (PrecipitationType.HeavyFog, DiceRoller.D4(1));
                        }
                        else if (percent <= 50)
                        {
                            return (PrecipitationType.MediumSnow, DiceRoller.D4(1));
                        }
                        else if (percent <= 90)
                        {
                            return (PrecipitationType.MediumSnow, DiceRoller.D8(1));
                        }
                        return (PrecipitationType.MediumSnow, DiceRoller.D12(2));
                    }
                case PrecipitationIntensity.Heavy:
                    if (temperature > 32)
                    {
                        if (percent <= 10)
                        {
                            return (PrecipitationType.HeavyFog, DiceRoller.D8(1));
                        }
                        else if (percent <= 20)
                        {
                            return (PrecipitationType.HeavyFog, DiceRoller.D6(2));
                        }
                        else if (percent <= 50)
                        {
                            return (PrecipitationType.HeavyRain, DiceRoller.D12(1));
                        }
                        else if (percent <= 70)
                        {
                            return (PrecipitationType.HeavyRain, DiceRoller.D12(2));
                        }
                        else if (percent <= 85)
                        {
                            return (temperature < 40 ? PrecipitationType.Sleet : PrecipitationType.HeavyRain, DiceRoller.D8(1));
                        }
                        else if (percent <= 90)
                        {
                            return (PrecipitationType.Thunderstorm, 1);
                        }
                        return (PrecipitationType.Thunderstorm, DiceRoller.D3(1));
                    }
                    else
                    {
                        if (percent <= 10)
                        {
                            return (PrecipitationType.MediumFog, DiceRoller.D8(1));
                        }
                        else if (percent <= 20)
                        {
                            return (PrecipitationType.HeavyFog, DiceRoller.D6(2));
                        }
                        else if (percent <= 60)
                        {
                            return (PrecipitationType.LightSnow, DiceRoller.D12(2));
                        }
                        else if (percent <= 90)
                        {
                            return (PrecipitationType.MediumSnow, DiceRoller.D8(1));
                        }
                        return (PrecipitationType.HeavySnow, DiceRoller.D6(1));
                    }
                case PrecipitationIntensity.Torrential:
                    if (temperature > 32)
                    {
                        if (percent <= 5)
                        {
                            return (PrecipitationType.HeavyFog, DiceRoller.D8(1));
                        }
                        else if (percent <= 10)
                        {
                            return (PrecipitationType.HeavyFog, DiceRoller.D6(2));
                        }
                        else if (percent <= 30)
                        {
                            return (PrecipitationType.HeavyRain, DiceRoller.D6(2));
                        }
                        else if (percent <= 60)
                        {
                            return (PrecipitationType.HeavyRain, DiceRoller.D12(2));
                        }
                        else if (percent <= 80)
                        {
                            return (temperature < 40 ? PrecipitationType.Sleet : PrecipitationType.HeavyRain, DiceRoller.D6(2));
                        }
                        else if (percent <= 95)
                        {
                            return (PrecipitationType.Thunderstorm, DiceRoller.D3(1));
                        }
                        return (PrecipitationType.Thunderstorm, DiceRoller.D6(1));
                    }
                    else
                    {
                        if (percent <= 5)
                        {
                            return (PrecipitationType.HeavyFog, DiceRoller.D8(1));
                        }
                        else if (percent <= 10)
                        {
                            return (PrecipitationType.HeavyFog, DiceRoller.D6(2));
                        }
                        else if (percent <= 50)
                        {
                            return (PrecipitationType.HeavySnow, DiceRoller.D4(1));
                        }
                        else if (percent <= 90)
                        {
                            return (PrecipitationType.HeavySnow, DiceRoller.D8(1));
                        }
                        return (PrecipitationType.HeavySnow, DiceRoller.D12(2));
                    }
            }
            throw new NotImplementedException();
        }

    }
}
