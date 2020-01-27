using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Nu.OfficerMiniGame.Dal.Dto
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Season
    {
        Spring,
        Summer,
        Fall,
        Winter
    }
}
