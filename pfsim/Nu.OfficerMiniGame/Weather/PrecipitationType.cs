using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Nu.OfficerMiniGame.Weather
{
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
        Blizzard,
        Thundersnow,

        Sandstorm,
        Hail,
        Hurricane,
        Tornado
    }
}
