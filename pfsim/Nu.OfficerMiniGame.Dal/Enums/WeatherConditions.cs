using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Nu.OfficerMiniGame
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum WeatherConditions
    {
        FairWinds,
        Calm,
        Clear,
        Drizzle,
        Rain,
        HeavyRain,
        HeavySeas,
        Storms,
        Gales,
        Hurricane,
        Fog
    }
}
