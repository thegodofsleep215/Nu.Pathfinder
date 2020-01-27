using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Nu.OfficerMiniGame.Dal.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum DaysOfTheWeek
    {
        Moonday,
        Toliday,
        Wealday,
        Oathday,
        Fireday,
        Starday,
        Sunday,
    }
}
