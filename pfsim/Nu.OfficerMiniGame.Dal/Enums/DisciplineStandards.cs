using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Nu.OfficerMiniGame
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum DisciplineStandards
    {
        Lax,
        Normal,
        Strict
    }
}
