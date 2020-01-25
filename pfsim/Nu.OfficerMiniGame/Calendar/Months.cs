using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Nu.OfficerMiniGame.Calendar
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Months
    {
        Abadius,
        Calistril,
        Pharast,
        Gozran,
        Desnus,
        Sarenith,
        Erastus,
        Arodus,
        Rova,
        Lamashan,
        Neth,
        Kuthona
    }

}
