using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Nu.OfficerMiniGame
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum ShipSize
    {
        Large,
        Huge,
        Gargantuan,
        Colossal,
        Medium
    }
}
