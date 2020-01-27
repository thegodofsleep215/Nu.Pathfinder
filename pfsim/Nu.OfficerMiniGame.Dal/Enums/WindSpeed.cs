using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Nu.OfficerMiniGame.Dal.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum WindSpeed
    {
        Light,
        Moderate,
        Strong,
        Severe,
        Windstorm
    }

}
