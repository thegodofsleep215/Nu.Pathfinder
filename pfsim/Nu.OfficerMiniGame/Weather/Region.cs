using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Nu.OfficerMiniGame.Weather
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Region
    {
        Cold,
        Temperate,
        Tropical,
    }
}
