using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Nu.OfficerMiniGame.Weather
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum PrecipitationIntensity
    {
        Light,
        Medium,
        Heavy,
        Torrential
    }
}
